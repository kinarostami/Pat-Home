using CoreLayer.Services.Sliders;
using DomainLayer.Models.Sliders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Areas.Admin.Pages.Sliders
{
    //[PermissionChecker(Permissions.مدیریت_اسلایدر_ها)]

    public class IndexModel : PageModel
    {
        private readonly ISliderService _slider;

        public IndexModel(ISliderService slider)
        {
            _slider = slider;
        }
        public List<ShopSlider> Sliders { get; set; }
        public void OnGet()
        {
            Sliders = _slider.GetSliders().OrderByDescending(d => d.CreationDate).ToList();
        }

        public async Task<IActionResult> OnGetDeleteSlider(long sliderId)
        {
            try
            {
                await _slider.DeleteSlider(sliderId);
                return Content("Deleted");
            }
            catch
            {
                return Content("Error");
            }
        }
    }
}
