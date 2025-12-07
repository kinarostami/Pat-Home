
namespace DomainLayer.Models
{
    public class SiteSetting:BaseEntity
    {
        #region SiteInfo
        public string ShopTitle { get; set; }
        public string MagTitle { get; set; }
        public string PersianSitName { get; set; }
        public string EnglishSitName { get; set; }
        public string PhoneNumber { get; set; }
        public string TelePhone { get; set; }
        public string Address { get; set; }
        public string ShopDescription { get; set; }
        public string MagDescription { get; set; }
        public string BaseSiteUrl { get; set; }

        #endregion
        #region SotioalNetworks
        public string InsTaGram { get; set; }
        public string Telegram { get; set; }
        public string LinkDin { get; set; }
        public string Twitter { get; set; }
        #endregion

        #region Email
        public string Email { get; set; }
        public string EmailPassword { get; set; }
        public int EmailSmtpPort { get; set; }
        public string EmailSmtpServer { get; set; }
        #endregion
    }
}