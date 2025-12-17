using CoreLayer.Services.Emails;
using CoreLayer.Services.Users;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Pages.Auth;

[ValidateAntiForgeryToken]
public class ForgetPasswordModel : PageUtil
{
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;
    public ForgetPasswordModel(IUserService userService, IEmailService emailService)
    {
        _userService = userService;
        _emailService = emailService;
    }

    public async Task<IActionResult> OnGet()
    {
        if (User.Identity.IsAuthenticated) return Redirect("/");
        return Page();
    }

    public async Task<IActionResult> OnPost(string email)
    {
        return await TryCatch(async () =>
        {
            var user = await _userService.GetSingleUserByEmail(email);
            if (user == null)
                throw new Exception("ایمیل وارد شده نامعتبر است");
            _emailService.SendForgotPassword(user);
        }, successReturn: "/Auth/Login", successTitle: "ایمیل با موفقیت ارسال شد", successMessage: "وارد ایمیل خود شوید و روی لینک تغییر کلمه عبور کلیک کنید");
    }
}
