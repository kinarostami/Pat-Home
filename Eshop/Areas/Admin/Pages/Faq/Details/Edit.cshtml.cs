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
    public class EditModel : PageModel
    {
        private readonly IFaqService _faqService;

        public EditModel(IFaqService faqService)
        {
            _faqService = faqService;
        }
        [BindProperty]
        public FaqDetail FaqDetail { get; set; }
        public async Task<IActionResult> OnGet(long faqId,long detailId)
        {
            var faq = await _faqService.GetFaqById(faqId);
            if (faq == null) RedirectToPage("Index", new {faqId = faqId});
            FaqDetail =await _faqService.GetFaqDetail(detailId);
            if (FaqDetail == null) RedirectToPage("Index", new { faqId = faqId });

            return Page();
        }
        public async Task<IActionResult> OnPost(long faqId, long detailId)
        {
            try
            {
                FaqDetail.FaqId = faqId;
                FaqDetail.Id = detailId;
                await _faqService.EditFaqDetail(FaqDetail);
                TempData["Success"] = "true";
                return RedirectToPage("Index", new { faqId = faqId });
            }
            catch
            {
                TempData["Error"] = "true";
                return RedirectToPage("Index", new { faqId = faqId });
            }
        }
    }
}
