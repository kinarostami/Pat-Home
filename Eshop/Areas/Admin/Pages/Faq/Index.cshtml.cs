using CoreLayer.Services.Faqs;
using CoreLayer.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages.Faq
{
    [PermissionChecker(Permissions.ادمین)]
    public class IndexModel : PageModel
    {
        private readonly IFaqService _faq;

        public IndexModel(IFaqService faq)
        {
            _faq = faq;
        }
        public List<DomainLayer.Models.FAQs.Faq> Faqs { get; set; }
        public async Task OnGet()
        {
            Faqs =await _faq.GetFaqs();
        }

        public async Task<IActionResult> OnGetDeleteFaq(long faqId)
        {
            try
            {
               await _faq.DeleteFaq(faqId);
                return Content("Deleted");
            }
            catch
            {
                return Content("Error");
            }
        }
    }
}
