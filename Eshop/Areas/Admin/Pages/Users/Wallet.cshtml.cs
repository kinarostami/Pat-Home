using Common.Application;
using CoreLayer.DTOs.Wallets;
using CoreLayer.Services.Users;
using CoreLayer.Services.Wallets;
using Common.Application.UserUtil;
using DomainLayer.Models.Wallets;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Areas.Admin.Pages.Users
{
    //[PermissionChecker(Permissions.ادمین)]
    //[ValidateAntiForgeryToken]
    public class WalletModel : PageUtil
    {
        private readonly IWalletService _walletService;
        private readonly IUserService _userService;
        private readonly ILogger<WalletModel> _logger;

        public WalletModel(IWalletService walletService, IUserService userService, ILogger<WalletModel> logger)
        {
            _walletService = walletService;
            _userService = userService;
            _logger = logger;
        }

        [BindProperty]
        public Wallet Wallet { get; set; }
        public WalletsFilterDto Wallets { get; set; }
        public int WalletAmount { get; set; }
        public async Task<IActionResult> OnGet(int pageId = 1, long userId = 0)
        {
            if (!await _userService.IsUserExist(userId))
                return RedirectToPage("Index");

            WalletAmount = await _walletService.BalanceWallet(userId);
            Wallets = await _walletService.GetWallets(pageId, userId, 1000);
            return Page();
        }
        public async Task<IActionResult> OnPost(int userId)
        {
            return await TryCatch(async () =>
            {
                Wallet.IsFinally = true;
                Wallet.UserId = userId;
                await _walletService.AddWallet(Wallet);
                _logger.LogError($"Charge Manual Wallet For User Id ={userId} _ Price = {Wallet.Amount.TooMan()} _ ChargedBy={User.GetUserId()}");
            }, successReturn: "/Admin/Users/Wallet/" + userId, errorReturn: "/Admin/Users/Wallet/" + userId);
        }
    }
}
