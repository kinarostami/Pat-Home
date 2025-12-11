using System.Threading.Tasks;

namespace CoreLayer.Services.SiteRules
{
    public interface ISiteRuleService
    {
        Task AddOrEdit(DomainLayer.Models.SiteRules rules);
        Task<DomainLayer.Models.SiteRules> GetRules();
        Task<DomainLayer.Models.SiteRules> GetRules(bool isTracked);
    }
}