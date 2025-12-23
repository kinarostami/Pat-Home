using CoreLayer.DTOs.Admin.Articles;
using CoreLayer.Services.Articles;
using CoreLayer.Utilities;
using DomainLayer.Models.Articles;
using DomainLayer.Models.Roles;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Eshop.Areas.Admin.Pages.Articles;
//[PermissionChecker(Permissions.مدیریت_مقالات)]

//[ValidateAntiForgeryToken]
public class EditModel : PageUtil
{
    private readonly IArticleServices _article;

    public EditModel(IArticleServices article)
    {
        _article = article;
    }

    [BindProperty]
    public AddArticleViewModel ArticleModel { get; set; }

    public List<ArticleGroup> MainGroups { get; set; }
    public List<ArticleGroup> SubGroups { get; set; }

    public async Task<IActionResult> OnGet(long articleId)
    {
        var article = await _article.GetArticleById(articleId);
        if (article == null) return RedirectToPage("Index");

        InitArticleModel(article);
        InitGroups();
        return Page();
    }

    public async Task<IActionResult> OnPost(long articleId)
    {
        if (!ModelState.IsValid)
        {
            InitGroups();
            TempData["Error"] = "true";
            return Page();
        }
        if (ArticleModel.Title != ArticleModel.OldTitle && await _article.IsSubjectExist(ArticleModel.Title))
        {
            ModelState.AddModelError("ArticleModel.Title", "عنوان انتخاب شده قبلا استفاده شده است");
            InitGroups();
            return Page();
        }
        if (ArticleModel.Url != ArticleModel.OldUrl && await _article.IsUrlIsExist(ArticleModel.Url))
        {
            ModelState.AddModelError("ArticleModel.Url", "آدرس وارد شده تکراری است");
            InitGroups();
            return Page();
        }

        ArticleModel.Id = articleId;
        var res = await _article.EditArticle(ArticleModel);
        if (res)
        {
            TempData["Success"] = "true";
            return RedirectToPage("Index");
        }

        TempData["Error"] = "true";
        InitGroups();
        return Page();
    }

    private void InitArticleModel(Article article)
    {
        ArticleModel = new AddArticleViewModel()
        {
            Body = article.Body,
            GroupId = article.GroupId,
            Id = article.Id,
            ImageName = article.ImageName,
            IsShow = article.IsShow,
            IsSpecial = article.IsSpecial,
            MetaDescription = article.MetaDescription,
            ParentGroupId = article.ParentGroupId ?? 0,
            Tags = article.Tags,
            Title = article.Title,
            UserId = article.UserId,
            Url = article.Url,
            OldTitle = article.Title,
            OldUrl = article.Url,
            DateRelease = article.DateReals.ToShortDateString()
        };
    }
    private void InitGroups()
    {
        var groups = _article.GetArticleGroups();
        MainGroups = groups.Where(g => g.ParentId == null).ToList();
        SubGroups = groups.Where(g => g.ParentId == ArticleModel.GroupId).ToList();
    }
}
