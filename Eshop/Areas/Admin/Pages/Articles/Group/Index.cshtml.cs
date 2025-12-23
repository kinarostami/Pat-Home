using CoreLayer.Services.Articles;
using CoreLayer.Utilities;
using DomainLayer.Models.Articles;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages.Articles.Group
{
    //[PermissionChecker(Permissions.مدیریت_مقالات)]
    public class IndexModel : PageUtil
    {
        private readonly IArticleServices _article;

        public IndexModel(IArticleServices article)
        {
            _article = article;
        }
        public IQueryable<ArticleGroup> ArticleGroups { get; set; }
        public void OnGet()
        {
            ArticleGroups = _article.GetArticleGroups();
        }

        public async Task<IActionResult> OnGetDeleteGroup(long groupId)
        {
            return await AjaxTryCatch(async () =>
            {
                 await _article.DeleteGroup(groupId);

            }, successReturn:"Deleted");
        }
    }
}
