using CoreLayer.Services.Banners;
using CoreLayer.Utilities;
using DomainLayer.Models.Banners;
using DomainLayer.Models.Roles;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Areas.Admin.Pages.Banners;

//[PermissionChecker(Permissions.مدیریت_بنر_ها)]
public class IndexModel : PageUtil
{
    private readonly IBannerService _bannerService;

    public IndexModel(IBannerService bannerService)
    {
        _bannerService = bannerService;
    }

    public List<Banner> Banners { get; set; }

    public void OnGet()
    {
        Banners = _bannerService.GetBanners().ToList();
    }

    public async Task<IActionResult> OnGetDeleteBanner(long bannerId)
    {
        try
        {
            await _bannerService.DeleteBanner(bannerId);
            return Content("Deleted");
        }
        catch
        {
            return Content("Error");
        }
    }
}
