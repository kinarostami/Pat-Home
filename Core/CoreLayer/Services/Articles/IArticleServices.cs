using CoreLayer.DTOs.Admin.Articles;
using CoreLayer.DTOs.Mag;
using DomainLayer.Models.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Articles;

public interface IArticleServices
{
    Task<List<ArticleCard>> GetPopularArticles();
    Task<MagMainPageViewModel> GetMainPageValues();
    Task<List<ArticleCard>> GetRelatedArticles(string groupTitle);
    Task<ArticleCategory> GetArticleForCategory(int pageId,int take,string search,string categoryName);
    Task<ArticlesViewModel> GetArticlesForAdmin(int pageId, long articleId, string articleTitle, string shortLink, string searchType);
    Task<bool> AddArticle(AddArticleViewModel articleModel);
    Task<bool> EditArticle(AddArticleViewModel articleModel);
    Task<Article> GetArticleByRelations(long articleId);
    Task<Article> GetArticleById(long articleId);
    Task<Article> GetArticleByUrl(string url);
    Task<bool> DeActiveArticle(long articleId);
    Task<bool> IsSubjectExist(string subject);
    Task<bool> IsUrlIsExist(string url);

    #region Group

    IQueryable<ArticleGroup> GetArticleGroups();
    Task AddGroup(ArticleGroup group);
    Task<ArticleGroup> GetGroupById(long articleId);
    Task EditGroup(ArticleGroup group);
    Task DeleteGroup(long groupId);

    #endregion

    #region Comment

    Task AddComment(ArticleComment comment);
    Task<ArticleCommentsViewModel> GetCommentsByFilter(int pageId, int take, string startDate, string endDate);
    Task<bool> DeleteComment(long userId, long commentId);

    #endregion
}
