using CoreLayer.Services;
using CoreLayer.Services.AboutUses;
using CoreLayer.Services.AdminMainPage;
using CoreLayer.Services.Articles;
using CoreLayer.Services.Banners;
using CoreLayer.Services.Contact;
using CoreLayer.Services.DiscountCodes;
using CoreLayer.Services.Emails;
using CoreLayer.Services.Faqs;
using CoreLayer.Services.Logs;
using CoreLayer.Services.Newsletters;
using CoreLayer.Services.Notifications;
using CoreLayer.Services.Orders;
using CoreLayer.Services.Products;
using CoreLayer.Services.Products.Groups;
using CoreLayer.Services.ShippingCosts;
using CoreLayer.Services.SiteRules;
using CoreLayer.Services.SiteSettings;
using CoreLayer.Services.Sliders;
using CoreLayer.Services.Tickets;
using CoreLayer.Services.Users;
using CoreLayer.Services.Users.UserAddresses;
using CoreLayer.Services.Users.UserCards;
using CoreLayer.Services.Users.UserPoints;
using CoreLayer.Services.Wallets;
using CoreLayer.Services.ZarinPal;
using DomainLayer.Models.Orders.DomainServices;
using Eshop.Infrastructure.Filters;
using Eshop.Infrastructure.Recaptcha;
using Microsoft.Extensions.DependencyInjection;

namespace Eshop.Infrastructure
{
    public class DependencyRegister
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<UserCompleted>();
            services.AddScoped<IZarinPalService, ZarinPalService>();
            services.AddScoped<IGoogleRecaptcha, GoogleRecaptcha>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IShippingCostDomainService,ShippingCostDomainService>();
            services.AddTransient<OrderReportToExcel>();
            //Other
            services.AddScoped<INewsletterService, NewsletterService>();
            services.AddScoped<IFaqService, FaqService>();
            services.AddScoped<IAboutUsService, AboutUsService>();
            services.AddScoped<IContactUsService, ContactUsService>();
            services.AddScoped<ISiteSettingsService, SiteSettingsService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAppContext, AppContext>();
            services.AddScoped<ISiteRuleService, SiteRuleService>();
            services.AddScoped<IAdminPageService, AdminPageService>();

            //User
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserPointService, UserPointService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserAddressesService, UserAddressesService>();
            services.AddScoped<IUserCardService, UserCardService>();
            services.AddScoped<IUserRoleService, UserRoleService>();

            //Shop
            services.AddScoped<IAmazingProductService, AmazingProductService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductGroupService, ProductGroupService>();
            services.AddScoped<ISliderService, SliderService>();
            services.AddScoped<IBannerService, BannerService>();
            services.AddScoped<IShippingCostService, ShippingCostService>();

            //Discount Code
            services.AddScoped<IDiscountCodeService, DiscountCodeService>();
            //Article
            services.AddScoped<IArticleServices, ArticleServices>();

            //Orders
            services.AddScoped<IOrderService, OrderService>();

            //UserPanel
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IWalletService, WalletService>();
        }
    }
}