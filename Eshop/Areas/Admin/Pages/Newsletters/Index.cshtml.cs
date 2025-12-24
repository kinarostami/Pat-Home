using CoreLayer.DTOs.Admin;
using CoreLayer.Services.Newsletters;
using CoreLayer.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages.Newsletters
{
    [PermissionChecker(Permissions.ادمین)]
    public class IndexModel : PageModel
    {
        private readonly INewsletterService _newsletter;

        public IndexModel(INewsletterService newsletter)
        {
            _newsletter = newsletter;
        }
        public NewslettersViewModel Newsletters { get; set; }
        public async Task OnGet(int pageId=1,string subject="",string startDate="",string endDate="")
        {
            Newsletters = await _newsletter.GetNewsletters(pageId, 20, startDate, endDate, subject);
        }
        public async Task<IActionResult> OnGetShowPage(long id)
        {
            if (id > 0)
            {
                var model = await _newsletter.GetNewsletters()
                    .SingleOrDefaultAsync(n => n.Id == id);
                return Partial("_PopupModel", model);

            }
            return Partial("_PopupModel", null);
        }
    }
}
