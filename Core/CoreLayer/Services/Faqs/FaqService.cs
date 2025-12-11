using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Context;
using DomainLayer.Models.FAQs;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.Services.Faqs
{
    public class FaqService:BaseService,IFaqService
    {
        public FaqService(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Faq>> GetFaqs()
        {
            return await Table<Faq>().Include(c=>c.Children).OrderByDescending(d=>d.CreationDate).ToListAsync();
        }

        public async Task<Faq> GetFaqById(long faqId)
        {
            return await GetById<Faq>(faqId, "Children");
        }

        public async Task InsertFaq(Faq faq)
        {
            Insert(faq);
            await Save();
        }

        public async Task EditFaq(Faq faq)
        {
            Update(faq);
            await Save();
        }

        public async Task DeleteFaq(long faqId)
        {
            var faq =await GetFaqById(faqId);
            Delete(faq);
            await Save();
        }

        public async Task<List<FaqDetail>> GetFaqDetails(long faqId)
        {
            return await Table<FaqDetail>().Where(f => f.FaqId == faqId).ToListAsync();
        }

        public async Task<FaqDetail> GetFaqDetail(long detailId)
        {
            return await GetById<FaqDetail>(detailId, "Faq");

        }

        public async Task InsertFaqDetail(FaqDetail detail)
        {
            Insert(detail);
            await Save();
        }

        public async Task EditFaqDetail(FaqDetail detail)
        {
            Update(detail);
            await Save();
        }

        public async Task DeleteFaqDetail(long detailId)
        {
            var detail = await GetFaqDetail(detailId);
            Delete(detail);
            await Save();
        }
    }
}