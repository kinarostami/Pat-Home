using Common.Application.UserUtil;
using CoreLayer.Services;
using CoreLayer.Services.DiscountCodes;
using CoreLayer.Services.Orders;
using CoreLayer.Services.Wallets;
using CoreLayer.Services.ZarinPal;
using DomainLayer.Models.Orders;
using DomainLayer.Models.Orders.DomainServices;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Pages.Payment;

[ValidateAntiForgeryToken]
public class IndexModel : PageUtil
{
    private readonly IOrderService _order;
    private readonly IDiscountCodeService _code;
    private readonly IAppContext _appContext;
    private readonly IZarinPalService _zarinPal;
    private readonly ILogger<IndexModel> _logger;
    private readonly IShippingCostDomainService _shippingCostDomainService;
    private readonly IWalletService _walletService;
    public IndexModel(IOrderService order, IDiscountCodeService code, IAppContext appContext, IZarinPalService zarinPal, ILogger<IndexModel> logger, IShippingCostDomainService shippingCostDomainService, IWalletService walletService)
    {
        _order = order;
        _code = code;
        _appContext = appContext;
        _zarinPal = zarinPal;
        _logger = logger;
        _shippingCostDomainService = shippingCostDomainService;
        _walletService = walletService;
    }

    public Order Order { get; set; }
    public bool IsChanged { get; set; }
    public async Task<IActionResult> OnGet()
    {
        var order = await _order.CheckOrderAndReturn(User.GetUserId());
        if (order.Item1 == null)
            return Redirect("/");
        if (order.Item1.Address == null)
            return Redirect("/Checkout");

        Order = order.Item1;
        IsChanged = order.Item2;
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var order = await _order.GetCurrentOrder(User.GetUserId());
        order.CalculateShippingConst(_shippingCostDomainService);

        if (order.ItemCount == 0)
            return Redirect("/");

        if (order.GetTotalPriceForPay() == 0)
        {
            return await TryCatch(async () =>
            {
                await _order.FinallyOrder(order);
            }, successReturn: "/Payment/Checkout", errorReturn: "/Payment/CheckOut");
        }

        var payment = await _zarinPal.CreatePaymentRequest(order.GetTotalPriceForPay(),
            $"پرداخت فاکتور شماره #{order.Id}",
            $"{_appContext.SiteBaseUrl}/Payment/Validate?orderId={order.Id}",
            _appContext.SiteSettings.Email,
            _appContext.SiteSettings.PhoneNumber);

        if (payment.Status == 100)
        {
            order.Authority = payment.Authority;
            await _order.UpdateOrder(order);
            return Redirect(payment.GateWayUrl);
        }
        TempData["Error"] = ResultModel.Error("مشکلی در عملیات رخ داده");
        return Page();
    }
    public async Task<IActionResult> OnGetSetWalletAmount()
    {
        var order = await _order.GetCurrentOrder(User.GetUserId());
        var walletBallance = await _walletService.BalanceWallet(User.GetUserId());

        //Toggled
        if (order.ByWallet)
            walletBallance = 0;
        order.SetWalletAmoutn(walletBallance);
        await _order.UpdateOrder(order);
        return Content("Ok");
    }

    public async Task<IActionResult> OnGetValidate(string authority, string status, long orderId)
    {
        if (string.IsNullOrEmpty(authority) || status.ToLower() != "ok")
        {
            TempData["Error"] = true;
            _logger.LogError($"Payment Error With Status NOK");
            return Redirect("/Payment/Checkout");
        }
        try
        {
            var order = await _order.GetOrderById(orderId);
            var verification = await _zarinPal.CreateVerificationRequest(authority, order.GetTotalPriceForPay());
            if (verification.Status == 100)
            {
                order.RefId = verification.RefId;
                await _order.FinallyOrder(order);
                TempData["Success"] = true;
            }
            else
            {
                TempData["Error"] = true;
                _logger.LogError($"Error Payment With Status = {verification.Status}");
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = true;
            _logger.LogError($"Payment Error -> {ex.Message}", ex);
        }
        return Redirect("/Payment/Checkout");
    }

    public async Task<IActionResult> OnGetApplyDisCountCode(string code)
    {
        var discountCode = await _code.GetDiscountCode(code);
        if (discountCode == null)
            return Content(DiscountCodeStatus.NotFound.ToString());
        if (discountCode.Count == discountCode.UsedCount)
            return Content(DiscountCodeStatus.No_Enough.ToString());
        if (discountCode.EndDate.Date < DateTime.Now.Date)
            return Content(DiscountCodeStatus.Ended.ToString());
        if (discountCode.StartDate.Date > DateTime.Now.Date)
            return Content(DiscountCodeStatus.ComingSoon.ToString());


        var order = await _order.GetCurrentOrder(User.GetUserId());
        if (order == null) return Content("Error");
        if (!string.IsNullOrWhiteSpace(order.DiscountTitle)) return Content("DiscountCode_Used");


        return await AjaxTryCatch(async () =>
        {
            if (discountCode.Price != null)
            {
                order.Discount = discountCode.Price;
            }
            else
            {
                order.DiscountPercentage = discountCode.Percentage;
            }
            discountCode.UsedCount += 1;
            order.DiscountTitle = code;

            await _code.EditNewCode(discountCode);
        }, successReturn: "Success");

    }
}
public enum DiscountCodeStatus
{
    NotFound,
    Ended,
    ComingSoon,
    DeActive,
    No_Enough
}
