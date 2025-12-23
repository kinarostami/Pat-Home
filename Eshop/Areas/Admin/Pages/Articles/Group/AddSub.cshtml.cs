using CoreLayer.Services.Articles;
using DomainLayer.Models.Articles;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Areas.Admin.Pages.Articles.Group
{
    //[PermissionChecker(Permissions.مدیریت_مقالات)]

    [ValidateAntiForgeryToken]
    public class AddSubModel : PageUtil
    {
        private readonly IArticleServices _article;

        public AddSubModel(IArticleServices article)
        {
            _article = article;
        }
        [BindProperty]
        public ArticleGroup ArticleGroup { get; set; }
        public async Task<IActionResult> OnGet(long parentId)
        {
            var group = await _article.GetGroupById(parentId);
            if (group == null || group.ParentId != null) return RedirectToPage("Index");

            return Page();

        }

        public async Task<IActionResult> OnPost(long parentId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var group = await _article.GetGroupById(parentId);
            if (group == null || group.ParentId != null) return RedirectToPage("Index");

            return await TryCatch(async () =>
            {
                ArticleGroup.ParentId = parentId;
                await _article.AddGroup(ArticleGroup);

            }, successReturn: "/Articles/Group", errorReturn: $"/Articles/Group/AddSub/{parentId}");

        }
    }
}
