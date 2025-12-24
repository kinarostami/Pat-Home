using CoreLayer.Services.DiscountCodes;
using Common.Application.DateUtil;
using CoreLayer.Utilities;
using DomainLayer.Models.DiscountCodes;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages.DiscountCodes
{
    //[PermissionChecker(Permissions.ادمین)]

    //[ValidateAntiForgeryToken]
    public class AddModel : PageUtil
    {
        private readonly IDiscountCodeService _code;

        public AddModel(IDiscountCodeService code)
        {
            _code = code;
        }
        [BindProperty]
        public DiscountCode DiscountCode { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string stDate,string endDate)
        {
            DiscountCode.StartDate = stDate.ToMiladi();
            DiscountCode.EndDate = endDate.ToMiladi();

            if (DiscountCode.Price == null && DiscountCode.Percentage == null)
            {
                ModelState.AddModelError("DiscountCode.Price","لطفا مقدار یا درصد تخفیف را وارد کنید");
                TempData["Error"] = true;
                return Page();
            }
            else
            {
                if (DiscountCode.Percentage == 0 && DiscountCode.Price==0)
                {
                    ModelState.AddModelError("DiscountCode.Price", "لطفا مقدار یا درصد تخفیف را وارد کنید");
                    TempData["Error"] = true;
                    return Page();
                }
                else
                {
                    if (DiscountCode.Price > 0) DiscountCode.Percentage = null;
                    if (DiscountCode.Percentage > 0) DiscountCode.Price = null;
                }
            }

            return await TryCatch(async () =>
            {
                await _code.AddNewCode(DiscountCode);

            },successReturn:"/Admin/DiscountCodes");
           
        }
    }
}
