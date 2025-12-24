using CoreLayer.Utilities;
using DomainLayer.Models.Articles;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using DomainLayer.Models.Roles;
using CoreLayer.Services.Articles;

namespace Eshop.Areas.Admin.Pages.Articles.Group
{
    [PermissionChecker(Permissions.مدیریت_مقالات)]

    [ValidateAntiForgeryToken]
    public class EditModel : PageUtil
    {
        private readonly IArticleServices _article;

        public EditModel(IArticleServices article)
        {
            _article = article;
        }
        [BindProperty]
        public ArticleGroup ArticleGroup { get; set; }
        public async Task<IActionResult> OnGet(long groupId)
        {
            ArticleGroup = await _article.GetGroupById(groupId);
            if (ArticleGroup == null) return RedirectToPage("Index");

            return Page();
        }

        public async Task<IActionResult> OnPost(long groupId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return await TryCatch(async () =>
            {
                var group = await _article.GetGroupById(groupId);
                ArticleGroup.ParentId = group.ParentId;
                ArticleGroup.Id = groupId;
                await _article.EditGroup(ArticleGroup);
            }, successReturn: "/Admin/Articles/Group", errorReturn: $"/Admin/Articles/Group/Edit/{groupId}");

        }
    }
}
