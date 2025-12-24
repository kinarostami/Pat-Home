using CoreLayer.Services.ShippingCosts;
using DomainLayer.Models.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eshop.Areas.Admin.Pages.ShippingCosts
{
    public class IndexModel : PageModel
    {
        private readonly IShippingCostService _shippingCostService;

        public IndexModel(IShippingCostService shippingCostService)
        {
            _shippingCostService = shippingCostService;
        }
        public List<ShippingCost> ShippingCosts { get; set; }
        public async Task OnGet()
        {
            ShippingCosts =await _shippingCostService.GetShippingCosts();
        }
    }
}
