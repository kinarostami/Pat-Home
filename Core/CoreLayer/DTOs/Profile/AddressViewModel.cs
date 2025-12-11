using System.ComponentModel.DataAnnotations;

namespace CoreLayer.DTOs.Profile
{
    public class AddressViewModel
    {
        public long Id { get; set; } = 0;
        public long UserId { get; set; } = 0;
        [Required(ErrorMessage = "کد پستی را وارد کنید")]
        [MaxLength(10,ErrorMessage ="کد پستی نامعتبر است")]
        [MinLength(10,ErrorMessage = "کد پستی نامعتبر است")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = " آدرس را وارد کنید")]
        [MinLength(10, ErrorMessage = "آدرس پستی را بصورت کامل وارد کنید")]
        public string Address { get; set; }
        [Required(ErrorMessage = " شهر را وارد کنید")]
        public string City { get; set; }
        [Required(ErrorMessage = " استان را وارد کنید")]
        public string Shire { get; set; }
        [Required(ErrorMessage = " نام گیرنده را وارد کنید")]
        public string Name { get; set; }
        [Required(ErrorMessage = " نام خانوادگی گیرنده را وارد کنید")]
        public string Family { get; set; }
        [Required(ErrorMessage = " شماره تلفن را وارد کنید")]
        [MaxLength(11, ErrorMessage = " شماره تلفن نامعتبر است")]
        [MinLength(11, ErrorMessage = " شماره تلفن نامعتبر است")]
        public string Phone { get; set; }
        [Required(ErrorMessage = " کد ملی را وارد کنید")]
        [MaxLength(10, ErrorMessage = " کد ملی نامعتبر است")]
        [MinLength(10, ErrorMessage = " کد ملی نامعتبر است")]
        public string NationalCode { get; set; }
    }
}
