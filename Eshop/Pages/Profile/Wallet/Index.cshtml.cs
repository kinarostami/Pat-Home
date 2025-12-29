using CoreLayer.DTOs.Wallets;
using CoreLayer.Services;
using CoreLayer.Services.Wallets;
using CoreLayer.Services.ZarinPal;
using Common.Application.UserUtil;
using Eshop.Infrastructure;
using Eshop.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;
using DomainLayer.Models.Wallets;

namespace Eshop.Pages.Profile.Wallet
{
    [ServiceFilter(typeof(UserCompleted))]
    public class IndexModel : PageUtil
    {
        private readonly IWalletService _walletService;
        private readonly IAppContext _appContext;
        private readonly IZarinPalService _zarinPal;

        public IndexModel(IWalletService walletService, IAppContext appContext, IZarinPalService zarinPal)
        {
            _walletService = walletService;
            _appContext = appContext;
            _zarinPal = zarinPal;
        }
        public WalletsFilterDto WalletsFilter { get; set; }
        public int WalletAmount { get; set; }
        public async Task OnGet(int pageId = 1)
        {
            WalletsFilter = await _walletService.GetWallets(pageId, User.GetUserId(), 10);
            WalletAmount = await _walletService.BalanceWallet(User.GetUserId());
        }

        public async Task<IActionResult> OnPost(int amount)
        {
            var wallet = new DomainLayer.Models.Wallets.Wallet()
            {
                Amount = amount,
                CreationDate = DateTime.Now,
                Description = $"شارژ کیف پول",
                IsFinally = false,
                UserId = User.GetUserId(),
                WalletType = WalletType.واریز
            };
            var walletId = await _walletService.AddWallet(wallet);
            var payment =await _zarinPal.CreatePaymentRequest(wallet.Amount
                , wallet.Description,
                $"{_appContext.SiteBaseUrl}/profile/wallet/validate?id={walletId}",
                _appContext.SiteSettings.PhoneNumber,
                _appContext.SiteSettings.Email);
            if (payment.Status == 100)
            {
                return Redirect(payment.GateWayUrl);
            }
            TempData["Error"] = ResultModel.Error("مشکلی در عملیات رخ داده");
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnGetValidate(long id, string authority, string status)
        {
            return await TryCatch(async () =>
            {
                if (string.IsNullOrEmpty(authority) || status.ToLower() != "ok")
                    throw new Exception("تراکنش ناموقق");

                var wallet = await _walletService.GetWalletById(id);
                var verification = await _zarinPal.CreateVerificationRequest(authority, wallet.Amount);

                if (verification.Status != 100)
                {
                    throw new Exception($"پرداخت ناموفق  {verification.RefId}");
                }

                wallet.RefId = verification.RefId;
                wallet.Description += " کد پیگیری خرید : " + verification.RefId;
                await _walletService.FinallyWallet(wallet);
            }, successReturn: "/Profile/Wallet",
                successTitle: "پرداخت با موفقیت انجام شد",
                errorReturn: "/Profile/Wallet",
                errorMessage: "تراکنش ناموقق !");
        }
    }
}
