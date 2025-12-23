using CoreLayer.DTOs.Admin.Articles;
using CoreLayer.Services.Articles;
using CoreLayer.Utilities;
using DomainLayer.Models.Roles;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Areas.Admin.Pages.Articles;

//[PermissionChecker(Permissions.مدیریت_مقالات)]
public class IndexModel : PageUtil
{
    private readonly IArticleServices _articleServices;

    public IndexModel(IArticleServices articleServices)
    {
        _articleServices = articleServices;
    }

    public ArticlesViewModel ArticlesViewModel { get; set; }

    public async Task OnGet(int pageId = 1, long articleId = 0, string title = "",string shortLink = "",string serachType = "all")
    {
        ArticlesViewModel = await _articleServices.GetArticlesForAdmin(pageId, articleId, title, shortLink, serachType);
    }

    public async Task<IActionResult> OnGetDelete(long articleId)
    {
        var res = await _articleServices.DeActiveArticle(articleId);
        return res ? Content("Success") : Content("Error");
    }
}
