using System.Collections.Generic;
using System.Threading.Tasks;
using DomainLayer.Models.FAQs;

namespace CoreLayer.Services.Faqs
{
    public interface IFaqService
    {
        Task<List<Faq>> GetFaqs();
        Task<Faq> GetFaqById(long faqId);
        Task InsertFaq(Faq faq);
        Task EditFaq(Faq faq);
        Task DeleteFaq(long faqId);
        //Detail ->
        Task<List<FaqDetail>> GetFaqDetails(long faqId);
        Task<FaqDetail> GetFaqDetail(long detailId);
        Task InsertFaqDetail(FaqDetail detail);
        Task EditFaqDetail(FaqDetail detail);
        Task DeleteFaqDetail(long detailId);
    }
}