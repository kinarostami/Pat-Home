using System.Threading.Tasks;
using CoreLayer.Services.Articles;
using Common.Application.SecurityUtil;
using CoreLayer.Utilities;
using DomainLayer.Enums;
using DomainLayer.Models.Articles;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Areas.Admin.Pages.Articles.Group
{
    [PermissionChecker(Permissions.مدیریت_مقالات)]

    [ValidateAntiForgeryToken]
    public class EditModel : PageUtil
    {
        private readonly IArticleService _article;

        public EditModel(IArticleService article)
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
