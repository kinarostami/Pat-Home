using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLayer.DTOs.Admin;
using DomainLayer.Models.Articles;
using DomainLayer.Models.Newsletters;
using DomainLayer.Models.Products;

namespace CoreLayer.Services.Newsletters
{
    public interface INewsletterService
    {
        IQueryable<Newsletter> GetNewsletters();
        Task<NewslettersViewModel> GetNewsletters(int pageId,int take,string startDate,string endDate,string subject);
        Task<bool> RegisterToNewsLetter(string email);
        Task<bool> DeleteFromNewsLetter(string email,Guid memberCode);
        Task<List<NewsletterMember>> GetMembers();
        Task InsertNewNewsletter(Newsletter newsletter, bool sendEmail);
        Task SendNewsletters();
        Task SendMessageForArticle(Article article);
    }
}