using CoreLayer.DTOs.Admin.Products;
using CoreLayer.Services.Products;
using CoreLayer.Services.Products.Groups;
using CoreLayer.Utilities;
using Common.Application.UserUtil;
using DomainLayer.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages.Products
{
    //[PermissionChecker(Permissions.مدیریت_محصولات)]
    [ValidateAntiForgeryToken]
    public class AddModel : PageModel
    {
        private readonly IProductService _product;
        private readonly IProductGroupService _group;


        public AddModel(IProductService product, IProductGroupService @group)
        {
            _product = product;
            _group = @group;
        }
        [BindProperty]
        public AddProductViewModel ProductModel { get; set; }
        public IEnumerable<ProductGroup> MainGroups { get; set; }
        public IEnumerable<ProductGroup> SubParentGroups { get; set; }
        public IEnumerable<ProductGroup> ParentGroups { get; set; }

        public void OnGet()
        {
            MainGroups = _group.GetProductGroups().Where(g => g.ParentId == null);
            SubParentGroups = new List<ProductGroup>();
            ParentGroups = new List<ProductGroup>();
        }

        public async Task<IActionResult> OnPost()
        {
            ProductModel.Product.ProductTitle = ProductModel.Product.ProductTitle.Replace("/", "-");
            if (ProductModel.Product.SubParentGroupId == 0)
            {
                ProductModel.Product.SubParentGroupId = null;
            }

            if (ProductModel.CoverImage == null)
            {
                SetDefaultValues();
                ModelState.AddModelError(nameof(ProductModel.CoverImage), "بنر محصول را انتخاب کنید");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                SetDefaultValues();
                return Page();
            }

            ProductModel.Product.UserId = User.GetUserId();
            var res = await _product.AddNewProduct(ProductModel);
            if (!res)
            {
                SetDefaultValues();
                TempData["Error"] = true;
                return Page();
            }

            TempData["Success"] = "true";
            return RedirectToPage("Index");
        }

        #region Utilities

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

        private void SetDefaultValues()
        {
            var groups = _group.GetProductGroups();
            MainGroups = groups.Where(g => g.ParentId == null);
            ParentGroups = groups.Where(g => g.ParentId == ProductModel.Product.GroupId);
            SubParentGroups = groups.Where(g => g.ParentId == ProductModel.Product.ParentGroupId);
        }

        #endregion
    }
}
