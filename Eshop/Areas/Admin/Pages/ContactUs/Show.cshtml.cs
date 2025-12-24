using CoreLayer.Services.Contact;
using CoreLayer.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages.ContactUs
{
    [PermissionChecker(Permissions.پشتیبان)]

    [ValidateAntiForgeryToken]
    public class ShowModel : PageModel
    {
        private readonly IContactUsService _contact;

        public ShowModel(IContactUsService contact)
        {
            _contact = contact;
        }
        [BindProperty]
        public DomainLayer.Models.ContactUs ContactUs { get; set; }
        public async Task<IActionResult> OnGet(long contactId)
        {
            ContactUs =await _contact.GetContactUsById(contactId);
            if (ContactUs == null)
            {
                return RedirectToPage("Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(long contactId)
        {
            var contact = await _contact.GetContactUsById(contactId);
            contact.Answer = ContactUs.Answer;
            try
            {
               await _contact.SendAnswer(contact);
            }
            catch
            {
                ContactUs = contact;
                TempData["Error"] = "true";
                return Page();
            }
            TempData["Success"] = "true";
            return RedirectToPage("Index");
        }
    }
}
