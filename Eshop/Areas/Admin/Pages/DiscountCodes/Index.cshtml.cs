using CoreLayer.Services.DiscountCodes;
using CoreLayer.Utilities;
using DomainLayer.Models.DiscountCodes;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages.DiscountCodes
{
    [PermissionChecker(Permissions.ادمین)]

    public class IndexModel : PageUtil
    {
        private readonly IDiscountCodeService _code;

        public IndexModel(IDiscountCodeService code)
        {
            _code = code;
        }

        public IQueryable<DiscountCode> DiscountCodes { get; set; }
        public void OnGet()
        {
            DiscountCodes = _code.GetAllCodes();
        }

        public async Task<IActionResult> OnGetDeleteCode(long codeId)
        {
            return await AjaxTryCatch(async () =>
            {
                await _code.DeleteCode(codeId);

            },successReturn: "Deleted");
          
        }

        public async Task<IActionResult> OnGetIsCodeExist(string code)
        {
            return Content(await _code.IsCodeExist(code)?"Yes":"No");
        }
    }
}
