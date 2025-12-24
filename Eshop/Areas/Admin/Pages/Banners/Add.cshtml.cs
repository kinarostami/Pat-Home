using CoreLayer.Services.Banners;
using CoreLayer.Utilities;
using DomainLayer.Models.Banners;
using DomainLayer.Models.Roles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Areas.Admin.Pages.Banners;

//[PermissionChecker(Permissions.مدیریت_بنر_ها)]
//[ValidateAntiForgeryToken]
public class AddModel : PageModel
{
    private readonly IBannerService _banner;

    public AddModel(IBannerService banner)
    {
        _banner = banner;
    }
    [BindProperty]
    public Banner Banner { get; set; }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost(IFormFile imageFile)
    {
        Banner.Positions = BannerPositions.بالای_اسلایدر_فروشگاه;
        await _banner.AddBanner(Banner, imageFile);
        TempData["Success"] = "true";
        return RedirectToPage("Index");

    }
}
