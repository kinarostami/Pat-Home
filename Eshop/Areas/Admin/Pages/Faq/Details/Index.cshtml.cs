using CoreLayer.Services.Faqs;
using CoreLayer.Utilities;
using DomainLayer.Models.FAQs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages.Faq.Details
{
    [PermissionChecker(Permissions.ادمین)]

    public class IndexModel : PageModel
    {
        private readonly IFaqService _faqService;

        public IndexModel(IFaqService faqService)
        {
            _faqService = faqService;
        }
        public List<FaqDetail> FaqDetails { get; set; }
        public async Task<IActionResult> OnGet(long faqId)
        {
            var faq = await _faqService.GetFaqById(faqId);
            if (faq == null) return Redirect("/Admin/Faq");

            ViewData["faqId"] = faqId;
            FaqDetails = await _faqService.GetFaqDetails(faqId);
            return Page();
        }
        public async Task<IActionResult> OnGetDeleteDetail(long detailId)
        {
            try
            {
                await _faqService.DeleteFaqDetail(detailId);
                return Content("Deleted");
            }
            catch
            {
                return Content("Error");
            }
        }
    }
}
