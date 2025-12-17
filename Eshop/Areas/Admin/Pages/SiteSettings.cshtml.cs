using CoreLayer.DTOs;
using CoreLayer.Services.SiteSettings;
using CoreLayer.Utilities;
using DomainLayer.Models.Roles;
using Eshop.Infrastructure;
using Eshop.Static;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Areas.Admin.Pages
{
    [PermissionChecker(Permissions.ادمین)]
    [ValidateAntiForgeryToken]
    public class SiteSettingsModel : PageUtil
    {
        private readonly ISiteSettingsService _settings;

        public SiteSettingsModel(ISiteSettingsService settings)
        {
            _settings = settings;
        }
        [BindProperty]
        public AddSiteSettingsViewModel SiteSetting { get; set; }
        public async Task OnGet()
        {
            await InitData();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var siteSettings = await _settings.GetSiteSettings();
            if (siteSettings == null)
            {
                return await TryCatch(async () =>
                {
                    var res = await _settings.AddSiteSetting(SiteSetting);
                    AppStatic.SiteSettings = res;

                },successReturn:"/Admin",errorReturn:"/Admin");
            }

            return await TryCatch(async () =>
            {
                SiteSetting.Id = siteSettings.Id;
                var res = await _settings.EditSiteSetting(SiteSetting);
                AppStatic.SiteSettings = res;

            }, successReturn: "/Admin", errorReturn: "/Admin");
        }

        private async Task InitData()
        {
            var setting = await _settings.GetSiteSettings();
            if (setting == null) return;

            SiteSetting = new AddSiteSettingsViewModel()
            {
                Address = setting.Address,
                BaseSiteUrl = setting.BaseSiteUrl,
                Email = setting.Email,
                EmailPassword = setting.EmailPassword,
                EnglishSitName = setting.EnglishSitName,
                Id = setting.Id,
                Instagram = setting.InsTaGram,
                LinkDin = setting.LinkDin,
                PersianSitName = setting.PersianSitName,
                PhoneNumber = setting.PhoneNumber,
                Twitter = setting.Twitter,
                TelePhone = setting.TelePhone,
                Telegram = setting.Telegram,
                EmailSmtpPort = setting.EmailSmtpPort,
                EmailSmtpServer = setting.EmailSmtpServer,
                MagDescription = setting.MagDescription,
                MagSiteTitle = setting.MagTitle,
                ShopDescription = setting.ShopDescription,
                ShopSiteTitle = setting.ShopTitle,
            };
        }
    }
}
