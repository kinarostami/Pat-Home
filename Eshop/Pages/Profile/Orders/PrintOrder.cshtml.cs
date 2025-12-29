using System.Threading.Tasks;
using CoreLayer.Services.Orders;
using Common.Application.UserUtil;
using DomainLayer.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Pages.Profile.Orders
{
    [Authorize]
    public class PrintOrderModel : PageModel
    {
        private readonly IOrderService _orderService;

        public PrintOrderModel(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public Order Order { get; set; }
        public async Task<IActionResult> OnGet(long orderId)
        {
            
            Order = await _orderService.GetOrderById(orderId, User.GetUserId());
            if (Order == null)
                return Redirect("/Profile/Orders");

            return Page();
        }
    }
}
