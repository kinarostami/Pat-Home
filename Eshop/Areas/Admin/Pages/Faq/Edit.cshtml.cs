using CoreLayer.Services.Faqs;
using CoreLayer.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages.Faq
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
        public DomainLayer.Models.FAQs.Faq Faq { get; set; }
        public async Task<IActionResult> OnGet(long faqId)
        {
            Faq =await _faqService.GetFaqById(faqId);
            if (Faq != null)
            {
                return Page();
            }
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPost(long faqId)
        {
            try
            {
                Faq.Id = faqId;
                await _faqService.EditFaq(Faq);
                TempData["Success"] = "true";
                return RedirectToPage("Index");
            }
            catch 
            {
                TempData["Error"] = "true";
                return RedirectToPage("Index");
            }
        }
    }
}
