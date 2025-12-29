using Common.Application.UserUtil;
using CookieManager;
using CoreLayer.DTOs.Shop;
using CoreLayer.Services.Orders;
using CoreLayer.Services.Products;
using DomainLayer.Models.Orders;
using Eshop.Infrastructure;
using InventoryManagement.Application.ApplicationSercices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Pages.ShopCart;

public class IndexModel : PageUtil
{
    private readonly IOrderService _order;
    private readonly ICookieManager _cookie;
    private readonly IInventoryService _inventoryService;
    private readonly IProductService _productService;
    public IndexModel(IOrderService order, ICookieManager cookie, IInventoryService inventoryService, IProductService productService)
    {
        _order = order;
        _cookie = cookie;
        _inventoryService = inventoryService;
        _productService = productService;
    }

    public Order Order { get; set; }
    public bool IsChanged { get; set; }
    public async Task OnGet()
    {
        if (User.Identity.IsAuthenticated)
        {
            var model = await _order.CheckOrderAndReturn(User.GetUserId());
            Order = model.Item1;
            IsChanged = model.Item2;
        }
        else
        {
            Order = CookieJobs.GenerateFakeOrder(_cookie, _inventoryService, _productService);
        }
    }

    public async Task<IActionResult> OnGetDeleteDetail(long id)
    {
        return await AjaxTryCatch(async () =>
        {
            if (User.Identity.IsAuthenticated)
            {
                await _order.DeleteOrderDetail(id, User.GetUserId());
            }
            else
            {
                CookieJobs.DeleteItemFromShopCart(_cookie, id);
            }
        }, successReturn: "Success");
    }

    public async Task<IActionResult> OnGetChangeCount(long id, int count)
    {
        return await AjaxTryCatch(async () =>
        {
            await _order.ChangeDetailCount(User.GetUserId(), count, id);
        }, successReturn: "Success");
    }
    public async Task<IActionResult> OnGetShopCartItemCount()
    {
        if (User.Identity.IsAuthenticated)
        {
            var count = await _order.GetShopCartItemCount(User.GetUserId());
            return Content(count.ToString());
        }
        else
        {
            if (HttpContext.Request.Cookies["ShopCart"] != null)
            {
                var shopCart = _cookie.Get<List<AddProductToCartDto>>("ShopCart");
                return Content(shopCart.Sum(s => s.Count).ToString());
            }
            return Content("0");
        }
    }
}
