using CoreLayer.DTOs.Auth;
using CoreLayer.Services.Orders;
using CoreLayer.Services.Users;
using Eshop.Infrastructure;
using Eshop.Infrastructure.Recaptcha;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Eshop.Pages.Auth;

//[ValidateAntiForgeryToken]
public class LoginModel : PageUtil
{
    private readonly IUserService _userService;
    private readonly IOrderService _orderService;
    private readonly IGoogleRecaptcha _gorecaptcha;
    private readonly CookieManager.ICookieManager _cookieManager;
    public LoginModel(IUserService userService, IOrderService orderService, IGoogleRecaptcha gorecaptcha, CookieManager.ICookieManager cookieManager)
    {
        _userService = userService;
        _orderService = orderService;
        _gorecaptcha = gorecaptcha;
        _cookieManager = cookieManager;
    }

    [BindProperty]
    public LoginDto Login { get; set; }

    public IActionResult OnGet(string returnUrl)
    {
        if (returnUrl != null)
        {
            Login = new LoginDto()
            {
                ReturnUrl = returnUrl
            };
        }

        if (User.Identity.IsAuthenticated)
            return Redirect("/");

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        //if (!await _gorecaptcha.IsSatisfy())
        //{
        //    ModelState.AddModelError("", "اعتبار سنجی captcha نا موفق بود!");
        //    return Page();
        //}

        if (!ModelState.IsValid)
            return Page();

        return await TryCatch(async () =>
        {
           var user = await _userService.LoginUser(Login);
            if (user == null)
                throw new Exception("مشخصات وارد شده نامعتبر است");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,user.Email??"null"),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.MobilePhone,user.PhoneNumber),
                new Claim(ClaimTypes.Name,user.Name??"null"),
                new Claim("Family",user.Family??"null"),
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties()
            {
                IsPersistent = true,
                RedirectUri = Login.ReturnUrl
            };
            await HttpContext.SignInAsync(principal,properties);

            #region SetCookie
            CookieJobs.SetUserCookie(HttpContext, user);
            await CookieJobs.SaveOrderCookieInDataBase(HttpContext,_orderService,_cookieManager, user.Id);
            #endregion
        }, successReturn: Login.ReturnUrl ?? "/", showAlert: false);
    }
}
