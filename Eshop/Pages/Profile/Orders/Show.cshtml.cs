using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLayer.Services.Orders;
using Common.Application.UserUtil;
using DomainLayer.Models.Orders;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Pages.Profile.Orders
{
    [Authorize]
    public class ShowModel : PageUtil
    {
        private readonly IOrderService _order;

        public ShowModel(IOrderService order)
        {
            _order = order;
        }
        public Order Order { get; set; }
        public async Task<IActionResult> OnGet(long orderId)
        {
            Order = await _order.GetOrderById(orderId, User.GetUserId());
            if (Order == null)
                return RedirectToPage("Index");
            return Page();
        }
        public async Task<IActionResult> OnGetReceivedProducts(int orderId)
        {
            return await AjaxTryCatch(async () =>
            {
                await _order.ChangeStatusToReceived(orderId, User.GetUserId());
            }, successReturn: "Success");
        }
    }
}
