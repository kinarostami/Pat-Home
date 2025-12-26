using CoreLayer.DTOs.Admin.Products;
using CoreLayer.Services.Products;
using CoreLayer.Services.Products.Groups;
using CoreLayer.Utilities;
using DomainLayer.Models.Products;
using DomainLayer.Models.Roles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Areas.Admin.Pages.Products
{
    //[PermissionChecker(Permissions.مدیریت_محصولات)]

    public class EditModel : PageModel
    {
        private readonly IProductService _product;
        private readonly IProductGroupService _group;

        public EditModel(IProductService product, IProductGroupService @group)
        {
            _product = product;
            _group = @group;
        }
        [BindProperty]
        public AddProductViewModel ProductModel { get; set; }
        public List<ProductGroup> MainGroups { get; set; }
        public List<ProductGroup> SubParentGroups { get; set; }
        public List<ProductGroup> ParentGroups { get; set; }
        public async Task<IActionResult> OnGet(long productId)
        {
            var product = await _product.GetProductById(productId);

            if (product == null) return RedirectToPage("Index");


            ProductModel = ConvertProductToCustomModel(product);
            SetDefaultValues(product);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long productId)
        {
            ProductModel.Product.ProductTitle = ProductModel.Product.ProductTitle.Replace("/", "-");
            ProductModel.Product.ProductId = productId;
            if (!ModelState.IsValid)
            {
                var product = await _product.GetProductById(productId);
                ProductModel.Product.ProductGalleries = product.Galleries.ToList();
                SetDefaultValues(product);
                return Page();
            }

            var res = await _product.EditProduct(ProductModel);
            if (!res)
            {
                var product = await _product.GetProductById(productId);
                ProductModel.Product.ProductGalleries = product.Galleries.ToList();
                SetDefaultValues(product);
                TempData["Error"] = true;
                return Page();
            }
            TempData["Success"] = "true";
            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnGetDeleteProduct(long productId, string rejectText)
        {
            var product = await _product.GetProductByIdWithRelations(productId, new[] { "OrderDetails", "Seller", "Galleries" });

            if (product == null) return BadRequest();


            return Content("Error");
        }


        #region Utilities
        private void SetDefaultValues(Product product)
        {
            MainGroups = _group.GetProductGroups().Where(g => g.ParentId == null).ToList();
            ParentGroups = _group.GetGroupsByParentId(product.GroupId).ToList();
            SubParentGroups = _group.GetGroupsByParentId(product.ParentGroupId).ToList();
            ViewData["imageName"] = product.ImageName;
        }
        private AddProductViewModel ConvertProductToCustomModel(Product product)
        {
            var model = new AddProductViewModel()
            {
                Product = new ProductModel()
                {
                    GroupId = product.GroupId,
                    ParentGroupId = product.ParentGroupId,
                    ProductDescription = product.ProductDescription,
                    ProductTitle = product.ProductTitle,
                    SubParentGroupId = product.SubParnetGroupId,
                    ProductGalleries = product.Galleries.ToList(),
                    Status = product.Status,
                    ProductId = product.Id,
                    MetaDescription = product.MetaDescription,
                    Tags = product.Tags,
                    ImageName = product.ImageName,
                    UserId = product.UserId,
                    Gram = product.Gram,
                    DiscountPercentage = product.DiscountPercentage
                },
            };
            foreach (var item in product.Specifications)
            {
                model.Keys.Add(item.Key);
                model.Values.Add(item.Value);
            }
            return model;
        }
        #endregion
    }
}
