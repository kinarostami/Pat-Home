using CoreLayer.Services.Products.Groups;
using DomainLayer.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Areas.Admin.Pages.Products.Groups
{
    //[PermissionChecker(Permissions.مدیریت_محصولات)]

    public class IndexModel : PageModel
    {
        private readonly IProductGroupService _group;

        public IndexModel(IProductGroupService @group)
        {
            _group = @group;
        }

        public IQueryable<ProductGroup> Groups { get; set; }
        public void OnGet()
        {
            Groups = _group.GetProductGroups();
        }

        public async Task<IActionResult> OnGetDeleteGroup(long groupId)
        {
            var res = await _group.DeleteGroup(groupId);
            return Content(res ? "Deleted" : "Error");
        }

        public IActionResult OnGetProductGroupOptions(int parentId)
        {
            var groups = _group.GetGroupsByParentId(parentId);
            var options = "<option value='0'>انتخاب کنید</option>";

            foreach (var group in groups)
            {
                options += @$"<option value={group.Id}>{group.GroupTitle}</option>";
            }
            return Content(options);
        }
    }
}
