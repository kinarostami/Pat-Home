using CoreLayer.Services.Faqs;
using CoreLayer.Utilities;
using DomainLayer.Models.FAQs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages.Faq.Details
{
    [PermissionChecker(Permissions.ادمین)]
    [ValidateAntiForgeryToken]
    public class AddModel : PageModel
    {
        private readonly IFaqService _faqService;

        public AddModel(IFaqService faqService)
        {
            _faqService = faqService;
        }
        [BindProperty]
        public FaqDetail FaqDetail { get; set; }
        public async Task<IActionResult> OnGet(long faqId)
        {
            var faq = await _faqService.GetFaqById(faqId);
            if (faq == null) RedirectToPage("Index", new { faqId = faqId });
            ViewData["faqId"] = faqId;
            return Page();
        }

        public async Task<IActionResult> OnPost(long faqId)
        {
            try
            {
                FaqDetail.FaqId = faqId;
                await _faqService.InsertFaqDetail(FaqDetail);
                TempData["Success"] = "true";
                return RedirectToPage("Index", new { faqId = faqId });
            }
            catch
            {
                TempData["Error"] = "true";
                return RedirectToPage("Index",new {faqId=faqId});
            }
        }
    }
}
