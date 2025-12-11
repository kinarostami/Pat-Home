using System.Threading.Tasks;
using CoreLayer.DTOs.Admin;
using DomainLayer.Models;

namespace CoreLayer.Services.Contact
{
    public interface IContactUsService
    {
        Task SendMessage(ContactUs contactUs);
        Task<ContactUsesViewModel> GetContactUses(int pageId,string startDate,string endDate);
        Task<ContactUs> GetContactUsById(long contactUsId);
        Task SendAnswer(ContactUs contact);
    }
}