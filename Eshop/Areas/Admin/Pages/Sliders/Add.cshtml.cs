using CoreLayer.Services.Sliders;
using DomainLayer.Models.Sliders;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Areas.Admin.Pages.Sliders
{
    //[PermissionChecker(Permissions.مدیریت_اسلایدر_ها)]

    [ValidateAntiForgeryToken]
    public class AddModel : PageUtil
    {
        private readonly ISliderService _slider;

        public AddModel(ISliderService slider)
        {
            _slider = slider;
        }
        [BindProperty]
        public ShopSlider Slider { get; set; }
        public void OnGet()
        {


        }

        public async Task<IActionResult> OnPost(IFormFile sliderFile)
        {
            return await TryCatch(async () =>
            {
                await _slider.AddSlider(Slider, sliderFile);

            }, successReturn: "/Admin/Sliders");


        }
    }
}
