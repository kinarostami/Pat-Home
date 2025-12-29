using System.Threading.Tasks;
using CoreLayer.DTOs;
using CoreLayer.Services.Users.UserPoints;
using Common.Application.UserUtil;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Pages.Profile.Points
{
    [Authorize]
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IUserPointService _pointService;

        public IndexModel(IUserPointService pointService)
        {
            _pointService = pointService;
        }
        public UserPointsFilter UserPoints { get; set; }
        public int Points { get; set; }
        public async Task OnGet(int pageId = 1)
        {
            Points = _pointService.BalancePoints(User.GetUserId());
            UserPoints = await _pointService.GetUserPoints(pageId, 15, User.GetUserId());
        }
        public async Task<IActionResult> OnPost()
        {
            await _pointService.ConvertPointToWalletAmount(User.GetUserId());
            TempData["Success"] = true;
            return RedirectToPage("../Wallet/Index");
        }
    }
}
