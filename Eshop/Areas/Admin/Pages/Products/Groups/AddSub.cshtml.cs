using CoreLayer.Services.Products.Groups;
using DomainLayer.Models.Products;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Areas.Admin.Pages.Products.Groups
{
    //[PermissionChecker(Permissions.مدیریت_محصولات)]
    [ValidateAntiForgeryToken]

    public class AddSubModel : PageUtil
    {
        private readonly IProductGroupService _group;

        public AddSubModel(IProductGroupService @group)
        {
            _group = @group;
        }
        [BindProperty]
        public ProductGroup ProductGroup { get; set; }

        public async Task<IActionResult> OnGet(long parentId)
        {
            var group = await _group.GetProductGroups().SingleOrDefaultAsync(g => g.Id == parentId);
            if (group == null) return RedirectToPage("Index");
            return Page();
        }
        public async Task<IActionResult> OnPost(long parentId, IFormFile imageFile)
        {
            if (string.IsNullOrEmpty(ProductGroup.GroupTitle))
            {
                ModelState.AddModelError("CourseGroup.GroupTitle", "عنوان گروه را وارد کنید");
                return Page();
            }

            return await TryCatch(async () =>
            {
                ProductGroup.ParentId = parentId;
                await _group.AddGroup(ProductGroup, imageFile);
            }, successReturn: "/Admin/Products/Groups");
        }
    }
}
