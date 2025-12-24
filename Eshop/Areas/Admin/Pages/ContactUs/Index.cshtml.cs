using CoreLayer.DTOs.Admin;
using CoreLayer.Services.Contact;
using CoreLayer.Utilities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages.ContactUs
{
    [PermissionChecker(Permissions.پشتیبان)]

    public class IndexModel : PageModel
    {
        private readonly IContactUsService _contact;

        public IndexModel(IContactUsService contact)
        {
            _contact = contact;
        }
        public ContactUsesViewModel ContactsModel { get; set; }
        public async Task OnGet(int pageId = 1, string startDate = "", string endDate = "")
        {
            ContactsModel = await _contact.GetContactUses(pageId, startDate, endDate);
        }
    }
}
