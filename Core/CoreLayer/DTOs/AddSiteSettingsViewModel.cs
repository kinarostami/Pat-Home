using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CoreLayer.DTOs
{
    public class AddSiteSettingsViewModel
    {
        public long Id { get; set; } = 0;
        [Required(ErrorMessage = "نام وبسایت را وارد کنید")]
        public string PersianSitName { get; set; }
        [Required(ErrorMessage = "نام وبسایت اجباری است")]
        public string EnglishSitName { get; set; }
        public string Instagram { get; set; }
        public string Telegram { get; set; }
        public string LinkDin { get; set; }
        public string Twitter { get; set; }
        public string PhoneNumber { get; set; }
        public string TelePhone { get; set; }
        public string Address { get; set; }
    
        [Required(ErrorMessage = "ایمیل پشتیبانی را وارد کنید")]
        public string Email { get; set; }
        [Required(ErrorMessage = "کلمه عبور ایمیل  را وارد کنید")]
        public string EmailPassword { get; set; }
        [Required]
        public string EmailSmtpServer { get; set; }
        [Required]
        public int EmailSmtpPort { get; set; }
        [Required(ErrorMessage = "عنوان سایت(آموزشگاه) را وارد کنید")]
        public string ShopSiteTitle { get; set; }
        [Required(ErrorMessage = "عنوان سایت(مگ) را وارد کنید")]
        public string MagSiteTitle { get; set; }

        [Required(ErrorMessage = "توضیحات صفحه اصلی(فروشگاه) را وارد کنید")]
        public string ShopDescription { get; set; }
        [Required(ErrorMessage = "توضیحات صفحه اصلی (مگ) را وارد کنید")]
        public string MagDescription { get; set; }
        public string BaseSiteUrl { get; set; }
    }
}