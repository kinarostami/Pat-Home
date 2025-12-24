using CoreLayer.Services.Faqs;
using CoreLayer.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages.Faq
{
    [PermissionChecker(Permissions.ادمین)]
    [ValidateAntiForgeryToken]
    public class AddModel : PageModel
    {
        private readonly IFaqService _faq;

        public AddModel(IFaqService faq)
        {
            _faq = faq;
        }
        [BindProperty]
        public DomainLayer.Models.FAQs.Faq Faq { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                await _faq.InsertFaq(Faq);
                TempData["Success"] = "true";
                return RedirectToPage("Index");
            }
            catch
            {
                TempData["Error"] = "true";
                return Page();
            }
        }
    }
}
