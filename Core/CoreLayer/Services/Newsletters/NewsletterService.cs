using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Application.DateUtil;
using Common.Application.FileUtil;
using CoreLayer.DTOs.Admin;
using CoreLayer.Services.Emails;
using Common.Application.FileUtil;
using DataLayer.Context;
using DomainLayer.Models.Articles;
using DomainLayer.Models.Newsletters;
using DomainLayer.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.Services.Newsletters
{
    public class NewsletterService : BaseService, INewsletterService
    {
        private readonly IEmailService _email;
        private readonly IAppContext _appContext;

        public NewsletterService(AppDbContext context, IEmailService email, IAppContext appContext) : base(context)
        {
            _email = email;
            _appContext = appContext;
        }


        public IQueryable<Newsletter> GetNewsletters()
        {
            return Table<Newsletter>()
                .Where(n => n.IsSend);
        }

        public async Task<NewslettersViewModel> GetNewsletters(int pageId, int take, string startDate, string endDate, string subject)
        {
            var result = GetNewsletters();
            var stDate = startDate.ToMiladi();
            var eDate = endDate.ToMiladi();
            if (!string.IsNullOrEmpty(subject))
            {
                result = result.Where(r => r.Subject.Contains(subject));
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                result = result.Where(r => r.CreationDate.Date >= stDate.Date);

            }
            if (!string.IsNullOrEmpty(endDate))
            {
                result = result.Where(r => r.CreationDate.Date <= eDate.Date);

            }
            var skip = (pageId - 1) * take;
            var pageCount = (int)Math.Ceiling(result.Count() / (double)take);
            var model = new NewslettersViewModel()
            {
                Newsletters = await result.OrderByDescending(d => d.CreatedBy).Skip(skip).Take(take).ToListAsync(),
                EndDate = (string.IsNullOrEmpty(endDate) ? null : (DateTime?)eDate),
                StartDate = (string.IsNullOrEmpty(startDate) ? null : (DateTime?)stDate),
                Subject = subject,
                MemberCounts = Table<NewsletterMember>().Count()
            };
            model.GeneratePaging(result, take, pageId);
            return model;
        }

        public async Task<bool> RegisterToNewsLetter(string email)
        {
            try
            {
                Insert(new NewsletterMember()
                {
                    Email = email,
                    CreationDate = DateTime.Now
                });
                await Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteFromNewsLetter(string email, Guid memberCode)
        {
            var entity = await _context.NewsletterMembers.SingleOrDefaultAsync(e => e.Email == email && e.MemberCode == memberCode);
            if (entity == null) return false;
            try
            {
                Delete(entity);
                await Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<NewsletterMember>> GetMembers()
        {
            return await Table<NewsletterMember>().ToListAsync();
        }

        public async Task InsertNewNewsletter(Newsletter newsletter, bool sendEmail)
        {
            try
            {
                Insert(newsletter);
                await Save();
            }
            finally
            {
                if (sendEmail)
                {
                    try
                    {
                        _email.SendNewsletterMessage(newsletter, await GetMembers());
                        newsletter.IsSend = true;
                    }
                    catch
                    {
                        newsletter.IsSend = false;
                    }
                    Update(newsletter);
                    await Save();
                }
            }
        }

        public async Task SendNewsletters()
        {
            var unSends = Table<Newsletter>().Where(n => !n.IsSend);
            foreach (var newsletter in unSends)
            {
                if (newsletter.CreationDate.Date > DateTime.Now.Date) continue;

                _email.SendNewsletterMessage(newsletter, await GetMembers());
                newsletter.IsSend = true;
                newsletter.CreationDate = DateTime.Now;
                Update(newsletter);
            }
            await Save();
        }

        public async Task SendMessageForArticle(Article article)
        {
            var Link =
                $"{_appContext.SiteBaseUrl}/Mag/post/{article.Url}";
            var body = $"<img style='width:100%' src='{_appContext.SiteBaseUrl}{Directories.GetArticleImage(article.ImageName)}'/><h1><a href='{Link}'>{article.Title}</a></h1><br/>{article.Body}<a href='{Link}'>جزئیات بیشتر...</a>";

            var newsletter = new Newsletter()
            {
                Body = body,
                CreationDate = DateTime.Now,
                CreatedBy = _appContext.CurrentUserId,
                Subject = $"پست جدید در {_appContext.SiteSettings.PersianSitName}",
                IsSend = false
            };
            if (article.DateReals.Date <= DateTime.Now.Date)
            {
                await InsertNewNewsletter(newsletter, true);

            }
            else
            {
                newsletter.CreationDate = article.DateReals;
                await InsertNewNewsletter(newsletter, false);
            }
        }
    }
}