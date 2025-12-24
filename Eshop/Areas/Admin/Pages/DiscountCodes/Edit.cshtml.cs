using CoreLayer.Services.DiscountCodes;
using Common.Application.DateUtil;
using CoreLayer.Utilities;
using DomainLayer.Models.DiscountCodes;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages.DiscountCodes
{
    [PermissionChecker(Permissions.ادمین)]

    [ValidateAntiForgeryToken]
    public class EditModel : PageUtil
    {
        private readonly IDiscountCodeService _code;

        public EditModel(IDiscountCodeService code)
        {
            _code = code;
        }
        [BindProperty]
        public DiscountCode DiscountCode { get; set; }
        [BindProperty]
        public string OldCodeTitle { get; set; }
        public async Task<IActionResult> OnGet(long codeId)
        {
            DiscountCode = await _code.GetDiscountCode(codeId);
            if (DiscountCode == null) return RedirectToPage("Index");
            OldCodeTitle = DiscountCode.CodeTitle;
            return Page();
        }
        public async Task<IActionResult> OnPost(long codeId, string stDate, string endDate)
        {
            DiscountCode.StartDate = stDate.ToMiladi();
            DiscountCode.EndDate = endDate.ToMiladi();
            if (DiscountCode.CodeTitle != OldCodeTitle)
            {
                if (await _code.IsCodeExist(DiscountCode.CodeTitle))
                {
                    ModelState.AddModelError("DiscountCode.CodeTitle", "عنوان کد تکرای است ، کد دیگری انتخاب کنید");
                    TempData["Error"] = true;
                    return Page();
                }
            }
            if (DiscountCode.Price == null && DiscountCode.Percentage == null)
            {
                ModelState.AddModelError("DiscountCode.Price", "لطفا مقدار یا درصد تخفیف را وارد کنید");
                TempData["Error"] = true;
                return Page();
            }
            else
            {
                if (DiscountCode.Percentage == 0 && DiscountCode.Price == 0)
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
                var code = await _code.GetDiscountCode(codeId);
                DiscountCode.Id = codeId;
                DiscountCode.UsedCount = code.UsedCount;
                await _code.EditNewCode(DiscountCode);
            }, successReturn: "/Admin/DiscountCodes", errorReturn: "/Admin/DiscountCodes/Edit/" + codeId);
        }
    }
}
