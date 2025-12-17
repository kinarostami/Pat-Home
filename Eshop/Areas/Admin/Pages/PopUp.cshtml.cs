using CoreLayer.Services;
using CoreLayer.Utilities;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages
{
    [PermissionChecker(Permissions.ادمین)]
    public class PopUpModel : PageUtil
    {
        private readonly IAppContext _appContext;

        public PopUpModel(IAppContext appContext)
        {
            _appContext = appContext;
        }
        [BindProperty]
        public DomainLayer.Models.PopUpModel PopUp { get; set; }
        public async Task OnGet()
        {
            PopUp = await _appContext.GetPopUp();
        }

        public async Task<IActionResult> OnPost()
        {
            return await TryCatch(async () =>
            {
                await _appContext.AddOrEditPopUp(PopUp);
            });
        }
    }
}
