using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Application.DateUtil;
using CoreLayer.DTOs.Admin;
using CoreLayer.Services.Emails;
using DataLayer.Context;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.Services.Contact
{
    public class ContactUsService : BaseService, IContactUsService
    {
        private readonly IEmailService _email;

        public ContactUsService(AppDbContext context, IEmailService email) : base(context)
        {
            _email = email;
        }

        public async Task SendMessage(ContactUs contactUs)
        {
            contactUs.IsSeenAdmin = false;
            contactUs.Answer = null;
            Insert(contactUs);
            await Save();
        }

        public async Task<ContactUsesViewModel> GetContactUses(int pageId, string startDate, string endDate)
        {
            var result = Table<ContactUs>();
            var take = 10;
            var stDate = startDate.ToMiladi();
            var eDate = endDate.ToMiladi();
            if (!string.IsNullOrEmpty(startDate))
            {
                result = result.Where(r => r.CreationDate.Date >= stDate.Date);
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                result = result.Where(r => r.CreationDate.Date <= eDate.Date);
            }
            var skip = (pageId - 1) * take;
            var model = new ContactUsesViewModel()
            {
                ContactUses = await result.OrderBy(d => d.IsSeenAdmin).Skip(skip).Take(take).ToListAsync(),
                EndDate = (string.IsNullOrEmpty(endDate) ? null : (DateTime?)eDate),
                StartDate = (string.IsNullOrEmpty(startDate) ? null : (DateTime?)stDate)
            };
            model.GeneratePaging(result,take,pageId);
            return model;
        }

        public async Task<ContactUs> GetContactUsById(long contactUsId)
        {
            return await _context.ContactUs.SingleOrDefaultAsync(c => c.Id == contactUsId);
        }

        public async Task SendAnswer(ContactUs contact)
        {
            _email.SendContactUsMessage(contact);
            contact.IsSeenAdmin = true;
            Update(contact);
            await Save();
        }
    }
}