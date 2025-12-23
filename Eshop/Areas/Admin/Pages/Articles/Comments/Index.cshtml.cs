using CoreLayer.DTOs.Mag;
using CoreLayer.Services.Articles;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Areas.Admin.Pages.Articles.Comments
{
    //[PermissionChecker(Permissions.مدیریت_مقالات)]
    public class IndexModel : PageModel
    {
        private readonly IArticleServices _article;

        public IndexModel(IArticleServices article)
        {
            _article = article;
        }
        public ArticleCommentsViewModel Comments { get; set; }
        public async Task OnGet(int pageId = 1, string startDate = "", string endDate = "")
        {
            Comments = await _article.GetCommentsByFilter(pageId, 20, startDate, endDate);
        }
    }
}
