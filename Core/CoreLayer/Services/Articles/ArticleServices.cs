using Common.Application;
using Common.Application.DateUtil;
using Common.Application.FileUtil;
using Common.Application.SecurityUtil;
using CoreLayer.DTOs.Admin.Articles;
using CoreLayer.DTOs.Mag;
using CoreLayer.Services.Newsletters;
using DataLayer.Context;
using DomainLayer.Models.Articles;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.Services.Articles;

public class ArticleServices : BaseService,IArticleServices
{
    private readonly INewsletterService _newsletterService;

    public ArticleServices(AppDbContext context, INewsletterService newsletterService) : base(context)
    {
        _newsletterService = newsletterService;
    }

    public async Task<bool> AddArticle(AddArticleViewModel articleModel)
    {
        if (articleModel.ImageSelector == null) return false;
        if (!articleModel.ImageSelector.IsImage()) return false;

        var article = ConvertViewModelToMainModel(articleModel);
        article.ShortLink = GenerateShortKey();
        article.ImageName = await SaveFileInServer.SaveFile(articleModel.ImageSelector, Directories.Article);
        try
        {
            Insert(article);
            await Save();
            //Send Email For NewsLetter Members
            await _newsletterService.SendMessageForArticle(article);

            return true;
        }
        catch
        {
            DeleteFileFromServer.DeleteFile(article.ImageName,Directories.Article);
            return false;
        }
    }

    public async Task AddComment(ArticleComment comment)
    {
        Insert(comment);
        await Save();
    }

    public async Task AddGroup(ArticleGroup group)
    {
        Insert(group);
        await Save();
    }

    public async Task<bool> DeActiveArticle(long articleId)
    {
        try
        {
            var article = await _context.Articles.FindAsync(articleId);
            article.IsShow = false;
            Update(article);
            await Save();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteComment(long userId, long commentId)
    {
        var comment = await _context.ArticleComments.SingleOrDefaultAsync(c => c.UserId == userId && c.Id == commentId);
        if (comment == null) return false;
        Delete(comment);
        await Save();
        return true;
    }

    public async Task DeleteGroup(long groupId)
    {
        var group = await _context.ArticleGroups.
                Include(c => c.ArticlesMainGroups)
                .Include(c => c.ArticlesParentGroups)
                .Include(c => c.ArticleGroups)
                .SingleOrDefaultAsync(a => a.Id == groupId);

        if (group == null) throw new Exception();
        if (group.ArticlesMainGroups.Any() || group.ArticlesParentGroups.Any()) throw new Exception();


        if (group.ArticleGroups.Any())
        {
            Delete(group.ArticleGroups);
        }
        Delete(group);
        await Save();
    }

    public async Task<bool> EditArticle(AddArticleViewModel articleModel)
    {
        var mainArticle = await GetById<Article>(articleModel.Id);
        if (mainArticle == null) return false;

        var imageSelectorImageName = "";
        var oldImage = mainArticle.ImageName;
        try
        {
            var article = ConvertViewModelToMainModel(articleModel);

            article.ShortLink = mainArticle.ShortLink;
            article.CreationDate = mainArticle.CreationDate;
            article.Visit = mainArticle.Visit;
            article.ImageName = mainArticle.ImageName;
            article.UserId = mainArticle.UserId;

            if (articleModel.ImageSelector != null)
            {
                if (articleModel.ImageSelector.IsImage())
                {
                    imageSelectorImageName = await SaveFileInServer.SaveFile(articleModel.ImageSelector, Directories.Article);
                    article.ImageName = imageSelectorImageName;
                }
            }
            Update(article);
            await Save();
            //Delete Old Image
            if (articleModel.ImageSelector != null)
            {
                if (articleModel.ImageSelector.IsImage())
                {
                    DeleteFileFromServer.DeleteFile(oldImage, Directories.Article);
                }
            }

            return true;
        }
        catch
        {
            DeleteFileFromServer.DeleteFile(imageSelectorImageName, Directories.Article);
            return false;
            throw;
        }
    }

    public async Task EditGroup(ArticleGroup group)
    {
        Update(group);
        await Save();
    }

    public async Task<Article> GetArticleById(long articleId)
    {
        return await GetById<Article>(articleId);
    }

    public async Task<Article> GetArticleByRelations(long articleId)
    {
        var article = await Table<Article>()
                .Include(c => c.User)
                .Include(c => c.MainGroup)
                .Include(c => c.ParentGroup)
                .Include(c => c.ArticleComments)
                .ThenInclude(c => c.User)
                .SingleOrDefaultAsync(a => a.Id == articleId && a.DateReals <= DateTime.Now);
        if (article != null)
        {
            article.Visit += 1;
            Update(article);
            await Save();
        }

        return article;
    }

    public async Task<Article> GetArticleByUrl(string url)
    {
        var article = await Table<Article>()
                .Include(c => c.User)
                .Include(c => c.MainGroup)
                .Include(c => c.ParentGroup)
                .Include(c => c.ArticleComments)
                .ThenInclude(c => c.User)
                .SingleOrDefaultAsync(a => a.Url == url.FixArticleUrl() && a.DateReals <= DateTime.Now);
        if (article != null)
        {
            article.Visit += 1;
            Update(article);
            await Save();
        }

        return article;
    }

    public async Task<ArticleCategory> GetArticleForCategory(int pageId, int take, string search, string categoryName)
    {
        var result = Table<Article>()
                .Include(t => t.User)
                .Include(t => t.MainGroup)
                .Include(t => t.ArticleComments)
                .Include(t => t.ParentGroup).Where(a => a.IsShow && a.DateReals <= DateTime.Now);

        if (!string.IsNullOrEmpty(search))
        {
            result = result.Where(r => r.Title.Contains(search) || r.Tags.Contains(search));
        }
        if (!string.IsNullOrEmpty(categoryName))
        {
            result = result.Where(r => r.MainGroup.EnglishTitle.Contains(categoryName) || r.ParentGroup.EnglishTitle.Contains(categoryName));
        }

        var skip = (pageId - 1) * take;
        var pageCount = (int)Math.Ceiling(result.Count() / (double)take);

        var articleCards = result.OrderByDescending(d => d.CreationDate).Skip(skip).Take(take).Select(a => new ArticleCard()
        {
            ImageName = a.ImageName,
            CreateDate = a.CreationDate,
            Description = (a.Body.ConvertHtmlToText().Length > 70
                ? a.Body.ConvertHtmlToText().Substring(0, 65) + "..."
                : a.Body.ConvertHtmlToText()),
            Title = (a.Title.Length > 50 ? a.Title.Substring(0, 46) + "..." : a.Title),
            ArticleId = a.Id,
            BuilderName = a.User.Name + " " + a.User.Family,
            CategoryName = (a.ParentGroup != null ? a.ParentGroup.GroupTitle : a.MainGroup.GroupTitle),
            Url = a.Url,
            EnglishGroupTitle = (a.ParentGroup != null ? a.ParentGroup.EnglishTitle : a.MainGroup.EnglishTitle),
        });
        return new ArticleCategory()
        {
            Articles = await articleCards.ToListAsync(),
            StartPage = (pageId - 4 <= 0) ? 1 : pageId - 4,
            EndPage = (pageId + 5 > pageCount) ? pageCount : pageId + 5,
            CurrentPage = pageId,
            PageCount = pageCount,
            EntityCount = result.Count(),
            CategoryTitle = categoryName,
            Search = search,
            ArticleGroups = Table<ArticleGroup>().ToList(),
            Take = take,
            Category = await Table<ArticleGroup>().FirstOrDefaultAsync(a => a.GroupTitle == categoryName)
        };
    }

    public IQueryable<ArticleGroup> GetArticleGroups()
    {
        return Table<ArticleGroup>();
    }

    public async Task<ArticlesViewModel> GetArticlesForAdmin(int pageId, long articleId, string articleTitle, string shortLink, string searchType)
    {
        var result = Table<Article>().Include(c => c.User).AsQueryable();

        if (!string.IsNullOrEmpty(articleTitle))
        {
            result = result.Where(r => r.Title.Contains(articleTitle));
        }
        if (!string.IsNullOrEmpty(searchType))
        {
            switch (searchType)
            {
                case "active":
                    result = result.Where(r => r.IsShow);
                    break;
                case "deActive":
                    result = result.Where(r => !r.IsShow);
                    break;
                default:
                    break;
            }
        }
        if (!string.IsNullOrEmpty(shortLink))
        {
            result = result.Where(r => r.ShortLink.Contains(shortLink));

        }
        if (articleId > 0)
        {
            result = result.Where(r => r.Id == articleId);
        }

        var take = 15;
        var skip = (pageId - 1) * take;
        var pageCount = (int)Math.Ceiling(result.Count() / (double)take);

        return new ArticlesViewModel()
        {
            Articles = await result.OrderByDescending(d => d.CreationDate).Skip(skip).Take(take).ToListAsync(),
            StartPage = (pageId - 4 <= 0) ? 1 : pageId - 4,
            EndPage = (pageId + 5 > pageCount) ? pageCount : pageId + 5,
            CurrentPage = pageId,
            PageCount = pageCount,
            EntityCount = result.Count(),
            Id = articleId,
            ShortLink = shortLink,
            Title = articleTitle,
            SearchType = searchType
        };
    }

    public async Task<ArticleCommentsViewModel> GetCommentsByFilter(int pageId, int take, string startDate, string endDate)
    {
        var result = Table<ArticleComment>()
                .Include(c => c.User)
                .Include(c => c.Article).AsQueryable();

        var stDate = startDate.ToMiladi();
        var eDate = endDate.ToMiladi();
        if (!string.IsNullOrEmpty(startDate))
        {
            result = result.Where(r => r.CreationDate.Date >= stDate.Date);
        }
        if (!string.IsNullOrEmpty(endDate))
        {
            result = result.Where(r => r.CreationDate.Date <= eDate.Date);
        }
        var skip = (pageId - 1) * take;
        var pageCount = (int)Math.Ceiling(result.Count() / (double)take);

        return new ArticleCommentsViewModel()
        {
            Comments = await result.OrderByDescending(d => d.CreationDate).Skip(skip).Take(take).ToListAsync(),
            StartPage = (pageId - 4 <= 0) ? 1 : pageId - 4,
            EndPage = (pageId + 5 > pageCount) ? pageCount : pageId + 5,
            CurrentPage = pageId,
            PageCount = pageCount,
            EntityCount = result.Count(),
            EndDate = (string.IsNullOrEmpty(endDate) ? null : (DateTime?)eDate),
            StartDate = (string.IsNullOrEmpty(startDate) ? null : (DateTime?)stDate)
        };
    }

    public async Task<ArticleGroup> GetGroupById(long articleId)
    {
        return await Table<ArticleGroup>().SingleOrDefaultAsync(s => s.Id == articleId);
    }

    public async Task<MagMainPageViewModel> GetMainPageValues()
    {
       var articles = Table<Article>().Where(a => a.IsShow && a.DateReals <= DateTime.Now);
            var last = await articles.OrderByDescending(d => d.CreationDate).Take(6).Select(a =>
                new ArticleCard()
                {
                    ImageName = a.ImageName,
                    CreateDate = a.CreationDate,
                    Description = (a.Body.ConvertHtmlToText().Length > 70
                        ? a.Body.ConvertHtmlToText().Substring(0, 65) + "..."
                        : a.Body.ConvertHtmlToText()),
                    Title = (a.Title.Length > 50 ? a.Title.Substring(0, 46) + "..." : a.Title),
                    ArticleId = a.Id,
                    BuilderName = a.User.Name + " " + a.User.Family,
                    CategoryName = (a.ParentGroup != null ? a.ParentGroup.GroupTitle : a.MainGroup.GroupTitle),
                    Url = a.Url,
                    EnglishGroupTitle = (a.ParentGroup != null ? a.ParentGroup.EnglishTitle : a.MainGroup.EnglishTitle),
                }).ToListAsync();
            var topVisit = await articles.OrderByDescending(d => d.Visit).Take(4).Select(a =>
                new ArticleCard()
                {
                    ImageName = a.ImageName,
                    CreateDate = a.CreationDate,
                    Description = (a.Body.ConvertHtmlToText().Length > 70
                        ? a.Body.ConvertHtmlToText().Substring(0, 65) + "..."
                        : a.Body.ConvertHtmlToText()),
                    Title = (a.Title.Length > 50 ? a.Title.Substring(0, 46) + "..." : a.Title),
                    ArticleId = a.Id,
                    BuilderName = a.User.Name + " " + a.User.Family,
                    CategoryName = (a.ParentGroup != null ? a.ParentGroup.GroupTitle : a.MainGroup.GroupTitle),
                    Url = a.Url,
                    EnglishGroupTitle = (a.ParentGroup != null ? a.ParentGroup.EnglishTitle : a.MainGroup.EnglishTitle),
                }).ToListAsync();
            var special = articles.Where(s => s.IsSpecial).Select(a =>
                new ArticleCard()
                {
                    ImageName = a.ImageName,
                    CreateDate = a.CreationDate,
                    Description = (a.Body.ConvertHtmlToText().Length > 70
                        ? a.Body.ConvertHtmlToText().Substring(0, 65) + "..."
                        : a.Body.ConvertHtmlToText()),
                    Title = (a.Title.Length > 50 ? a.Title.Substring(0, 46) + "..." : a.Title),
                    ArticleId = a.Id,
                    BuilderName = a.User.Name + " " + a.User.Family,
                    CategoryName = (a.ParentGroup != null ? a.ParentGroup.GroupTitle : a.MainGroup.GroupTitle),
                    Url = a.Url,
                    EnglishGroupTitle = (a.ParentGroup != null ? a.ParentGroup.EnglishTitle : a.MainGroup.EnglishTitle),
                });
            var popular = await articles.OrderByDescending(s => s.ArticleComments.Count).Select(a =>
                new ArticleCard()
                {
                    ImageName = a.ImageName,
                    CreateDate = a.CreationDate,
                    Description = (a.Body.ConvertHtmlToText().Length > 70
                        ? a.Body.ConvertHtmlToText().Substring(0, 65) + "..."
                        : a.Body.ConvertHtmlToText()),
                    Title = (a.Title.Length > 50 ? a.Title.Substring(0, 46) + "..." : a.Title),
                    ArticleId = a.Id,
                    BuilderName = a.User.Name + " " + a.User.Family,
                    CategoryName = (a.ParentGroup != null ? a.ParentGroup.GroupTitle : a.MainGroup.GroupTitle),
                    Url = a.Url,
                    EnglishGroupTitle = (a.ParentGroup != null ? a.ParentGroup.EnglishTitle : a.MainGroup.EnglishTitle),
                }).ToListAsync();
            return new MagMainPageViewModel()
            {
                LastArticles = last,
                TopVisitArticles = topVisit,
                SpecialArticles = await special.Take(4).ToListAsync(),
                PopularArticles = popular,
                SpecialArticlesTitle = special.Select(s => s.Title).ToList(),
                Categories = Table<ArticleGroup>()
                    .Include(c => c.ArticlesMainGroups)
                    .Include(c => c.ArticlesParentGroups)
            };
    }

    public async Task<List<ArticleCard>> GetPopularArticles()
    {
        return await Table<Article>()
                .Include(t => t.User)
                .Include(t => t.MainGroup)
                .Include(t => t.ArticleComments)
                .Include(t => t.ParentGroup)
                .Where(a => a.IsShow && a.DateReals <= DateTime.Now)
                .OrderByDescending(a => a.Visit).Take(6)
                .Select(a => new ArticleCard()
                {
                    ImageName = a.ImageName,
                    CreateDate = a.CreationDate,
                    Description = (a.Body.ConvertHtmlToText().Length > 70
                        ? a.Body.ConvertHtmlToText().Substring(0, 65) + "..."
                        : a.Body.ConvertHtmlToText()),
                    Title = (a.Title.Length > 50 ? a.Title.Substring(0, 46) + "..." : a.Title),
                    ArticleId = a.Id,
                    BuilderName = a.User.Name + " " + a.User.Family,
                    CategoryName = (a.ParentGroup != null ? a.ParentGroup.GroupTitle : a.MainGroup.GroupTitle),
                    Url = a.Url,
                    EnglishGroupTitle = (a.ParentGroup != null ? a.ParentGroup.EnglishTitle : a.MainGroup.EnglishTitle),
                }).ToListAsync();
    }

    public async Task<List<ArticleCard>> GetRelatedArticles(string groupTitle)
    {
        return await Table<Article>().Where(a => a.IsShow && a.DateReals <= DateTime.Now).Take(8).Select(a =>
                new ArticleCard()
                {
                    ImageName = a.ImageName,
                    CreateDate = a.CreationDate,
                    Description = (a.Body.ConvertHtmlToText().Length > 70
                        ? a.Body.ConvertHtmlToText().Substring(0, 65) + "..."
                        : a.Body.ConvertHtmlToText()),
                    Title = (a.Title.Length > 50 ? a.Title.Substring(0, 46) + "..." : a.Title),
                    ArticleId = a.Id,
                    BuilderName = a.User.Name + " " + a.User.Family,
                    CategoryName = (a.ParentGroup != null ? a.ParentGroup.GroupTitle : a.MainGroup.GroupTitle),
                    Url = a.Url,
                    EnglishGroupTitle = (a.ParentGroup != null ? a.ParentGroup.EnglishTitle : a.MainGroup.EnglishTitle),
                }).ToListAsync();
    }

    public async Task<bool> IsSubjectExist(string subject)
    {
        return await Table<Article>().AnyAsync(a => a.Title.ToLower().Trim() == subject.ToLower().Trim());
    }

    public async Task<bool> IsUrlIsExist(string url)
    {
        return await Table<Article>().AnyAsync(u => u.Url == url.FixArticleUrl());
    }

    #region Utilities

    private Article ConvertViewModelToMainModel(AddArticleViewModel articleModel)
    {
        return new Article()
        {
            Title = articleModel.Title,
            Body = articleModel.Body,
            GroupId = articleModel.GroupId,
            ImageName = articleModel.ImageName,
            MetaDescription = articleModel.MetaDescription,
            ParentGroupId = (articleModel.ParentGroupId == 0 ? null : articleModel.ParentGroupId),
            Tags = articleModel.Tags,
            UserId = articleModel.UserId,
            Visit = 0,
            CreationDate = DateTime.Now,
            IsShow = articleModel.IsShow,
            IsSpecial = articleModel.IsSpecial,
            Id = articleModel.Id,
            Url = articleModel.Url.FixArticleUrl(),
            DateReals = articleModel.DateRelease.ToMiladi()
        };
    }
    private string GenerateShortKey(int length = 4)
    {
        //در این جا یک کلید با طول دلخواه تولید میکنیم
        var key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, length);

        while (Table<Article>().Any(p => p.ShortLink == key))
        {
            //تا زمانی که کلید ساخته شده تکراری باشد این عملیات تکرار میشود
            key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, length);
        }
        //در آخر یک کلید غیره تکراری با طول دلخواه ساخته شده
        return key;
    }

    #endregion
}
