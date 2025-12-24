using CoreLayer.Services;
using CoreLayer.Services.Newsletters;
using CoreLayer.Utilities;
using Common.Application.UserUtil;
using DomainLayer.Models.Newsletters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages.Newsletters
{
    [PermissionChecker(Permissions.ادمین)]
    [ValidateAntiForgeryToken]
    public class AddModel : PageModel
    {
        private readonly INewsletterService _newsletter;
        private readonly IAppContext _appContext;

        public AddModel(INewsletterService newsletter, IAppContext appContext)
        {
            _newsletter = newsletter;
            _appContext = appContext;
        }
     
        [BindProperty]
        public Newsletter Newsletter { get; set; }
        public IActionResult OnGet()
        {
            var siteSetting = _appContext.SiteSettings;
            var members = _newsletter.GetMembers().Result;
            if (members.Count < 1)
            {
                TempData["MemberIsZero"] = true;
                return RedirectToPage("Index");
            }
            if (string.IsNullOrEmpty(siteSetting.Email) || string.IsNullOrEmpty(siteSetting.EmailSmtpServer))
            {
                TempData["EmailNotFound"] = true;
                return RedirectToPage("Index");
            }
            ViewData["MemberCount"] = members.Count;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                Newsletter.CreatedBy = User.GetUserId();
                await _newsletter.InsertNewNewsletter(Newsletter,true);
                TempData["Success"] = "true";
                return RedirectToPage("Index");
            }
            catch
            {
                TempData["Error"] = "true";
                return RedirectToPage("Index");
            }
        }
    }
}
