using System.Threading.Tasks;
using CoreLayer.DTOs.Profile;
using CoreLayer.Services.Users;
using Common.Application.UserUtil;
using Eshop.Infrastructure;
using Eshop.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Pages.Profile
{
    [ServiceFilter(typeof(UserCompleted))]

    [ValidateAntiForgeryToken]
    public class ChangePasswordModel : PageUtil
    {
        private readonly IUserService _userService;

        public ChangePasswordModel(IUserService userService)
        {
            _userService = userService;
        }
        [BindProperty]
        public ChangePasswordDto PasswordDto { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            PasswordDto.UserId = User.GetUserId();
            return await TryCatch(async () =>
            {
                await _userService.ChangePassword(PasswordDto);
            },successReturn: "/Profile");
        }
    }
}
