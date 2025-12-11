using System.Threading.Tasks;
using CoreLayer.DTOs;
using DataLayer.Context;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.Services.SiteSettings
{
    public class SiteSettingsService : BaseService, ISiteSettingsService
    {
        public SiteSettingsService(AppDbContext context) : base(context)
        {

        }

        public async Task<SiteSetting> AddSiteSetting(AddSiteSettingsViewModel addModel)
        {
            var mainModel = convertViewModelToMainModel(addModel);

            Insert(mainModel);
            await Save();
            return mainModel;
        }

        public async Task<SiteSetting> GetSiteSettings()
        {
            return await _context.SiteSettings.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<SiteSetting> EditSiteSetting(AddSiteSettingsViewModel editModel)
        {
            var mainModel = convertViewModelToMainModel(editModel);

            Update(mainModel);
            await Save();

            return mainModel;
        }

        private SiteSetting convertViewModelToMainModel(AddSiteSettingsViewModel viewModel)
        {
            return new SiteSetting()
            {
                Address = viewModel.Address,
                BaseSiteUrl = viewModel.BaseSiteUrl,
                Id = viewModel.Id,
                Email = viewModel.Email,
                PhoneNumber = viewModel.PhoneNumber,
                EnglishSitName = viewModel.EnglishSitName,
                Twitter = viewModel.Twitter,
                InsTaGram = viewModel.Instagram,
                LinkDin = viewModel.LinkDin,
                PersianSitName = viewModel.PersianSitName,
                TelePhone = viewModel.TelePhone,
                Telegram = viewModel.Telegram,
                EmailSmtpPort = viewModel.EmailSmtpPort,
                EmailSmtpServer = viewModel.EmailSmtpServer,
                EmailPassword = viewModel.EmailPassword,
                ShopDescription = viewModel.ShopDescription,
                MagDescription = viewModel.MagDescription,
                MagTitle = viewModel.MagSiteTitle,
                ShopTitle = viewModel.ShopSiteTitle,
            };
        }
    }
}