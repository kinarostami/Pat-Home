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
    public class EditModel : PageModel
    {
        private readonly IShippingCostService _shippingCostService;

        public EditModel(IShippingCostService shippingCostService)
        {
            _shippingCostService = shippingCostService;
        }
        public ShippingCost ShippingCost { get; set; }
        public async Task<IActionResult> OnGet(long id)
        {
            ShippingCost = await _shippingCostService.GetById(id);
            if (ShippingCost == null)
                return RedirectToPage("Index");
            return Page();
        }
        public async Task<IActionResult> OnPost(long id)
        {
            ShippingCost.Id = id;
            await _shippingCostService.Edit(ShippingCost);
            TempData["Success"] = true;
            return RedirectToPage("Index");
        }
    }
}
