using CoreLayer.DTOs.Admin.Products;
using CoreLayer.Services.Products;
using CoreLayer.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DomainLayer.Models.Roles;
using DomainLayer.Models.Products;
using static CoreLayer.Services.Products.IProductService;

namespace Eshop.Areas.Admin.Pages.Products
{
    [PermissionChecker(Permissions.مدیریت_محصولات)]

    public class IndexModel : PageModel
    {
        private readonly IProductService _product;

        public IndexModel(IProductService product)
        {
            _product = product;
        }

        public AdminProductsViewModel Products { get; set; }
        public async Task<IActionResult> OnGet(int pageId = 1, string title = "", string shortLink = "", ProductStatus status = 0, string groupTitle = "")
        {
            Products = await _product.GetProductsForAdmin(pageId, 15, title, shortLink, status, groupTitle);
            return Page();
        }

        public async Task<IActionResult> OnGetDeleteProduct(long productId)
        {
            var product = await _product.GetProductById(productId);
            if (product == null) return BadRequest();

            try
            {
                var res = await _product.DeleteProduct(product);
                switch (res)
                {
                    case DeleteTypes.FullDelete:
                        return Content("Deleted");
                    case DeleteTypes.NotFound:
                        return NotFound();
                    case DeleteTypes.SoftDelete:
                        return Content("SoftDeleted");
                    default:
                        break;
                }
            }
            catch
            {
                return Content("Error");

            }
            return Content("Error");
        }
    }
}
