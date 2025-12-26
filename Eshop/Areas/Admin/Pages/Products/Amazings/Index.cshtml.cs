using CoreLayer.Services.Products;
using DomainLayer.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Areas.Admin.Pages.Products.Amazings
{
    //[PermissionChecker(Permissions.مدیریت_محصولات)]
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IAmazingProductService _amazing;

        public IndexModel(IAmazingProductService amazing)
        {
            _amazing = amazing;
        }

        public IQueryable<AmazingProduct> AmazingProducts { get; set; }
        public async Task OnGet(string search)
        {
            await _amazing.DeleteAmazingProducts();
            AmazingProducts = _amazing.GetAmazingProducts(search);
            ViewData["search"] = search;

        }

        public async Task<IActionResult> OnPost(List<long> products)
        {
            var res = await _amazing.AddProductToAmazing(products);
            if (res)
            {
                TempData["Success"] = "true";
            }
            else
            {
                TempData["Error"] = "true";

            }
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnGetDeleteAmazing(int amazingId)
        {
            try
            {
                await _amazing.DeleteProductFromAmazing(amazingId);
                return Content("Deleted");
            }
            catch
            {
                return Content("Error");
            }
        }
    }
}
