using System.Collections.Generic;
using DomainLayer.Models;
using DomainLayer.Models.Newsletters;
using DomainLayer.Models.Orders;
using DomainLayer.Models.Tickets;
using DomainLayer.Models.Users;

namespace CoreLayer.Services.Emails
{
    public interface IEmailService
    {
        void SendForgotPassword(User user);
        void SendActiveCode(User user);
        void SendContactUsMessage(ContactUs contactUs);
        void SendEmailForAnswerTicket(Ticket ticket);
        void SendNewsletterMessage(Newsletter newsletter, List<NewsletterMember> members);
        void SendTrackingCode(Order order);
    }
}