using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLayer.DTOs.Profile;
using CoreLayer.Services.Orders;
using Common.Application.UserUtil;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Pages.Profile.Orders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IOrderService _order;

        public IndexModel(IOrderService order)
        {
            _order = order;
        }
        public UserOrdersFilter Orders { get; set; }
        public async Task OnGet(int pageId=1)
        {
            Orders = await _order.GetUserOrders(pageId, 6, User.GetUserId());
        }
    }
}
