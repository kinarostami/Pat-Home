using CoreLayer.Services.ShippingCosts;
using CoreLayer.Utilities;
using DomainLayer.Models.Orders;
using DomainLayer.Models.Roles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Eshop.Areas.Admin.Pages.ShippingCosts
{
    [BindProperties]
    [ValidateAntiForgeryToken]
    [PermissionChecker(Permissions.ادمین)]
    public class AddModel : PageModel
    {
        private readonly IShippingCostService _shippingCostService;

        public AddModel(IShippingCostService shippingCostService)
        {
            _shippingCostService = shippingCostService;
        }
        public ShippingCost ShippingCost { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            await _shippingCostService.Add(ShippingCost);
            TempData["Success"] = true;
            return RedirectToPage("Index");
        }
    }
}
