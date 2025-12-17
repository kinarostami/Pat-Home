using CoreLayer.DTOs.Auth;
using CoreLayer.Services.Users;
using DomainLayer.Models.Users;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Pages.Auth;

[ValidateAntiForgeryToken]
public class ResetPasswordModel : PageUtil
{
    private readonly IUserService _userService;

    public ResetPasswordModel(IUserService userService)
    {
        _userService = userService;
    }

    [BindProperty]
    public ResetPasswordDto ResetPassword { get; set; }

    public async Task<IActionResult> OnGet(string email,string activeCode)
    {
        if (User.Identity.IsAuthenticated) return Redirect("/");

        var user = await _userService.GetSingleUserByEmail(email);
        if (user == null)
            return Redirect("/");
        if (user.ActiveCode != activeCode)
            return Redirect("/");

        return Page();
    }

    public async Task<IActionResult> OnPost(string emial,string activeCode)
    {
        if (!ModelState.IsValid)
            return Page();
        
        return await TryCatch(async () =>
        {
            ResetPassword.Email = emial;
            ResetPassword.ActiveCode = activeCode;
            await _userService.ResetPassword(ResetPassword);
        },successReturn: "/Auth/Login");
    }
}
