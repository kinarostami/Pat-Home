using Common.Application.UserUtil;
using CoreLayer.DTOs.Mag;
using CoreLayer.Services.Articles;
using DomainLayer.Models.Articles;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Pages.Mag;

[ValidateAntiForgeryToken]
public class PostModel : PageUtil
{
    private readonly IArticleServices _articleServices;

    public PostModel(IArticleServices articleServices)
    {
        _articleServices = articleServices;
    }

    public Article Article { get; set; }
    public List<ArticleGroup> ArticleGroups { get; set; }
    public List<ArticleCard> Articles { get; set; }
    public async Task<IActionResult> OnGet(string url)
    {
        Article = await _articleServices.GetArticleByUrl(url);
        if (Article == null) return Partial("_NotFound");

        ArticleGroups = await _articleServices.GetArticleGroups().ToListAsync();
        Articles = await _articleServices.GetPopularArticles();
        return Page();
    }

    public async Task<IActionResult> OnPost(string comment, long articleId)
    {
        var article = await _articleServices.GetArticleById(articleId);
        if (article == null) return Partial("_NotFound");

        if (!User.Identity.IsAuthenticated)
        {
            TempData["Error"] = ResultModel.Error("برای ثبت نظر باید وارد حساب کاربری خود شوید");
            return RedirectToPage("post",new {url = article.Url});
        }
        return await TryCatch(async () =>
        {
            var commentModel = new ArticleComment()
            {
                ArticleId = articleId,
                CreationDate = DateTime.Now,
                Text = comment,
                UserId = User.GetUserId()
            };
            await _articleServices.AddComment(commentModel);
        }, successReturn: $"/mag/post/{article.Url}#comments", errorReturn: $"/mag/post/{article.Url}#comments");
    }
}
