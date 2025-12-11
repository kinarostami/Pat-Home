using System;
using System.Threading.Tasks;
using DataLayer.Context;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.Services.SiteRules
{
    public class SiteRuleService:BaseService,ISiteRuleService
    {
        public SiteRuleService(AppDbContext context) : base(context)
        {
        }

        public async Task AddOrEdit(DomainLayer.Models.SiteRules rules)
        {
            rules.LastModify = DateTime.Now;
            if (rules.Id >= 1)
            {
                Update(rules);
                await Save();
            }
            else
            {
                
                Insert(rules);
                await Save();
            }
        }

        public async Task<DomainLayer.Models.SiteRules> GetRules()
        {
            return await Table<DomainLayer.Models.SiteRules>().FirstOrDefaultAsync();
        }

        public async Task<DomainLayer.Models.SiteRules> GetRules(bool isTracked)
        {
            return await TableTracking<DomainLayer.Models.SiteRules>().FirstOrDefaultAsync();

        }
    }
}