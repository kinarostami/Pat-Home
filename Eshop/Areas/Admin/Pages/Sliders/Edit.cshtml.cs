using CoreLayer.Services.Sliders;
using CoreLayer.Utilities;
using DomainLayer.Models.Roles;
using DomainLayer.Models.Sliders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Areas.Admin.Pages.Sliders
{
    //[PermissionChecker(Permissions.مدیریت_اسلایدر_ها)]

    [ValidateAntiForgeryToken]
    public class EditModel : PageModel
    {
        private readonly ISliderService _slider;

        public EditModel(ISliderService slider)
        {
            _slider = slider;
        }
        [BindProperty]
        public ShopSlider Slider { get; set; }
        public async Task<IActionResult> OnGet(long sliderId)
        {
            Slider = await _slider.GetSliderById(sliderId);
            if (Slider == null) return RedirectToPage("Index");

            return Page();
        }

        public async Task<IActionResult> OnPost(long sliderId,IFormFile sliderFile)
        {
            try
            {
                var slider = await _slider.GetTrackingSlider(sliderId);
                slider.Url = Slider.Url;
                slider.IsActive = Slider.IsActive;
                await _slider.EditSlider(slider, sliderFile);
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
