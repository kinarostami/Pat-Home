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
    public class AddModel : PageUtil
    {
        private readonly IArticleServices _article;

        public AddModel(IArticleServices article)
        {
            _article = article;
        }
        [BindProperty]
        public ArticleGroup ArticleGroup { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }
            return await TryCatch(async () =>
            {
                await _article.AddGroup(ArticleGroup);

            }, successReturn: "/Articles/Group");

        }
    }
}
