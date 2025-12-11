using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CoreLayer.DTOs.Admin.Articles
{
    public class AddArticleViewModel
    {
        [Required(ErrorMessage = "آدرس مقاله را وارد کنید")]
        [MaxLength(450, ErrorMessage = "آدرس نباید بیشتر از 400 کاراکتر باشد")]
        public string Url { get; set; }
        public long Id { get; set; } = 0;
        public long UserId { get; set; }

        [Required(ErrorMessage = "عنوان مقاله را وارد کنید")]
        [MaxLength(350,ErrorMessage ="عنوان نباید بیشتر از 300 کاراکتر باشد")]
        public string Title { get; set; }

        [Required(ErrorMessage = "توضیحاتی برای متور های جستوجو")]
        [MaxLength(400,ErrorMessage = "این توضیحات نباید بیشتر از 400 کاراکتر باشد")]
        public string MetaDescription { get; set; }

        [Required(ErrorMessage = "متن مقاله را وارد کنید")]
        public string Body { get; set; }
        [Required(ErrorMessage = "کلمات کلیدی را وارد کنید")]
        [MaxLength(1000)]
        public string Tags { get; set; }
        public string ImageName { get; set; }
        public IFormFile ImageSelector { get; set; }
        [Range(1,9999999999999999999,ErrorMessage = "دسته بندی را انتخاب کنید")]
        public long GroupId { get; set; }
        public long? ParentGroupId { get; set; }

        public bool IsShow { get; set; }
        public bool IsSpecial { get; set; }
        [Required(ErrorMessage = "تاریخ انتشار را مشخص کنید")]
        public string DateRelease { get; set; }

        public string OldTitle { get; set; }
        public string OldUrl { get; set; }
    }
}