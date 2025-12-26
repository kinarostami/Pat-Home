using System.Threading.Tasks;
using CoreLayer.Services.Products.Groups;
using Common.Application.SecurityUtil;
using CoreLayer.Utilities;
using DomainLayer.Enums;
using DomainLayer.Models.Products;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages.Products.Groups
{
    //[PermissionChecker(Permissions.مدیریت_محصولات)]
    //[ValidateAntiForgeryToken]
    public class EditModel : PageUtil
    {
        private readonly IProductGroupService _group;

        public EditModel(IProductGroupService @group)
        {
            _group = @group;
        }
        [BindProperty]
        public ProductGroup ProductGroup { get; set; }
        public async Task<IActionResult> OnGet(long groupId)
        {
            ProductGroup = await _group.GetGroupById(groupId);
            if (ProductGroup == null) return RedirectToPage("Index");
            return Page();
        }

        public async Task<IActionResult> OnPost(long groupId, IFormFile imageFile)
        {
            var group = await _group.GetProductGroups().SingleOrDefaultAsync(g => g.Id == groupId);

            if (string.IsNullOrEmpty(ProductGroup.GroupTitle))
            {
                TempData["Error"] = true;
                ModelState.AddModelError("CourseGroup.GroupTitle", "عنوان را وارد کنید");
                return Page();
            }

            return await TryCatch(async () =>
            {
                ProductGroup.GroupImage = group.GroupImage;
                ProductGroup.ParentId = group.ParentId;
                ProductGroup.Id = groupId;
                await _group.EditGroup(ProductGroup, imageFile);
            }, successReturn: "/Admin/Products/Groups", errorReturn: $"/Admin/Products/Groups/Edit/{groupId}");
        }
    }
}
