using CoreLayer.DTOs.Auth;
using CoreLayer.Services.Users;
using Eshop.Infrastructure;
using Eshop.Infrastructure.Recaptcha;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Pages.Auth;

[ValidateAntiForgeryToken]
public class RegisterModel : PageUtil
{
    private readonly IUserService _userService;
    private readonly IGoogleRecaptcha _googleRecaptcha;
    public RegisterModel(IUserService userService, IGoogleRecaptcha googleRecaptcha)
    {
        _userService = userService;
        _googleRecaptcha = googleRecaptcha;
    }

    [BindProperty]
    public RegisterDto Register { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (!await _googleRecaptcha.IsSatisfy())
        {
            ModelState.AddModelError("", "اعتبارسنجی captcha نا موفق بود!");
            return Page();
        }
        if (!ModelState.IsValid)
        {
            return Page();
        }
        return await TryCatch(async () =>
        {
           await _userService.RegisterUser(Register);
        },successReturn:"/Auth/Login");
    }
}
