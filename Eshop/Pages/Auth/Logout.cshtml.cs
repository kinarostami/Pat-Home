using CoreLayer.Services.Users;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Pages.Auth;

public class LogoutModel : PageUtil
{
    public async Task<IActionResult> OnGet()
    {
        if (!User.Identity.IsAuthenticated)
            return Redirect("/");

        await HttpContext.SignOutAsync();
        HttpContext.Response.Cookies.Delete("userInfo");
        return Redirect("/");
    }
}
