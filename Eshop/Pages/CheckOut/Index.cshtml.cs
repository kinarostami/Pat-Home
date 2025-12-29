using Common.Application.UserUtil;
using CoreLayer.Services.Orders;
using CoreLayer.Services.Users.UserAddresses;
using DomainLayer.Models.Orders;
using DomainLayer.Models.Orders.DomainServices;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Pages.CheckOut;

//[Authorize]
public class IndexModel : PageUtil
{
    private readonly IOrderService _orderService;
    private readonly IShippingCostDomainService _shippingCostDomainService;
    public IndexModel(IOrderService orderService, IShippingCostDomainService userAddressesService)
    {
        _orderService = orderService;
        _shippingCostDomainService = userAddressesService;
    }

    public Order Order { get; set; }
    public bool IsChanged { get; set; }

    public async Task<IActionResult> OnGet()
    {
        var res = await _orderService.CheckOrderAndReturn(User.GetUserId());
        if (res.Item1 == null) return Redirect("/");

        res.Item1.CalculateShippingConst(_shippingCostDomainService);

        Order = res.Item1;
        IsChanged = res.Item2;
        return Page();
    }

    public async Task<IActionResult> OnPost(int addressId, int sendType)
    {
        return await TryCatch(async () =>
        {
            var order = await _orderService.GetCurrentOrder(User.GetUserId());
            order.CalculateShippingConst(_shippingCostDomainService);
            await _orderService.UpdateOrder(order, addressId);
        }, successReturn: "/payment", errorReturn: "/CheckOut", showAlert: false);
    }
}
