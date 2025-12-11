using System.Threading.Tasks;
using CoreLayer.DTOs;
using DomainLayer.Models;

namespace CoreLayer.Services.SiteSettings
{
    public interface ISiteSettingsService
    {
        Task<SiteSetting> AddSiteSetting(AddSiteSettingsViewModel addModel);
        Task<SiteSetting> GetSiteSettings();
        Task<SiteSetting> EditSiteSetting(AddSiteSettingsViewModel editModel);
    }
}