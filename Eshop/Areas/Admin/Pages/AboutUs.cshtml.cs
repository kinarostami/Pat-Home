using CoreLayer.Services.AboutUses;
using CoreLayer.Utilities;
using DomainLayer.Models;
using DomainLayer.Models.Roles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Areas.Admin.Pages
{
    [PermissionChecker(Permissions.ادمین)]
    public class AboutUsModel : PageModel
    {
        private readonly IAboutUsService _about;

        public AboutUsModel(IAboutUsService about)
        {
            _about = about;
        }
        [BindProperty]
        public AboutUs AboutUs { get; set; }
        public async Task OnGet()
        {
            AboutUs = await _about.GetAboutUs();
        }

        public async Task<IActionResult> OnPost()
        {

            var about = await _about.GetAboutUs();
            if (about != null)
            {
                about.Body = AboutUs.Body;
                await _about.AddOrEdit(about);
                TempData["Success"] = "true";
                return RedirectToPage("Index");
            }

            await _about.AddOrEdit(AboutUs);
            TempData["Success"] = "true";
            return RedirectToPage("Index");

        }
    }
}
