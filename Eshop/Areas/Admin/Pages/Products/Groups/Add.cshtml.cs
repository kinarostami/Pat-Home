using CoreLayer.Services.Products.Groups;
using CoreLayer.Utilities;
using DomainLayer.Models.Products;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages.Products.Groups
{
    //[PermissionChecker(Permissions.مدیریت_محصولات)]
    //[ValidateAntiForgeryToken]

    public class AddModel : PageUtil
    {
        private readonly IProductGroupService _group;

        public AddModel(IProductGroupService @group)
        {
            _group = @group;
        }
        [BindProperty]
        public ProductGroup ProductGroup { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(IFormFile imageFile)
        {
            if (string.IsNullOrEmpty(ProductGroup.GroupTitle))
            {
                ModelState.AddModelError("CourseGroup.GroupTitle", "عنوان گروه را وارد کنید");
                return Page();
            }
            return await TryCatch(async () =>
            {
                await _group.AddGroup(ProductGroup, imageFile);
            }, successReturn: "/Admin/Products/Groups");
        }
    }
}
