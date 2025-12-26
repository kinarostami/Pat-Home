using CoreLayer.Services.Orders;
using CoreLayer.Utilities;
using DomainLayer.Models.Orders;
using DomainLayer.Models.Roles;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Areas.Admin.Pages.Orders;

//[PermissionChecker(Permissions.ادمین)]
public class ShowModel : PageUtil
{
    private readonly IOrderService _orderService;

    public ShowModel(IOrderService orderService)
    {
        _orderService = orderService;
    }
    public Order Order { get; set; }

    public async Task<IActionResult> OnGet(long orderId)
    {
        Order = await _orderService.GetOrderById(orderId);
        if (Order == null) return RedirectToPage("Product");

        return Page();
    }

    public async Task<IActionResult> OnGetSendProduct(long orderId, string trackingCode)
    {
        var order = await _orderService.GetOrderById(orderId);
        if (order == null) return BadRequest();
        await _orderService.SendProduct(order, trackingCode);
        return Content("Success");
    }
}
