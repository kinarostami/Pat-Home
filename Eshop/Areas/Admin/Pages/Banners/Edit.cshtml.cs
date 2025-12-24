using CoreLayer.Services.Banners;
using CoreLayer.Utilities;
using DomainLayer.Models.Banners;
using DomainLayer.Models.Roles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Areas.Admin.Pages.Banners;

//[PermissionChecker(Permissions.مدیریت_بنر_ها)]
public class EditModel : PageModel
{
    private readonly IBannerService _banner;

    public EditModel(IBannerService banner)
    {
        _banner = banner;
    }

    [BindProperty]
    public Banner Banner { get; set; }
    public async Task<IActionResult> OnGet(long bannerId)
    {
        Banner = await _banner.GetBannerById(bannerId);
        if (Banner == null) return RedirectToPage("Index");

        return Page();
    }

    public async Task<IActionResult> OnPost(long bannerId, IFormFile imageFile)
    {
        try
        {
            var banner = await _banner.GetBannerById(bannerId);
            Banner.ImageName = banner.ImageName;
            Banner.Id = bannerId;
            Banner.Positions = BannerPositions.بالای_اسلایدر_فروشگاه;
            await _banner.EditBanner(Banner, imageFile);
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
