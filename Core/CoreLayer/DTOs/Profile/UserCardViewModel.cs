using System.ComponentModel.DataAnnotations;

namespace CoreLayer.DTOs.Profile
{
   public class UserCardViewModel
    {
        public long CardId { get; set; } = 0;
        public long UserId { get; set; } = 0;
        [Required(ErrorMessage ="شماره کارت را وارد کنید")]
        [MaxLength(16,ErrorMessage ="شماره حساب نامعتبر است")]
        [MinLength(16, ErrorMessage = "شماره حساب نامعتبر است")]
        public string CardNumber { get; set; }
        [Required(ErrorMessage = "شماره شبا را وارد کنید")]
        [MaxLength(24, ErrorMessage = "شماره شبا نامعتبر است")]
        [MinLength(24, ErrorMessage = "شماره شبا نامعتبر است")]
        public string ShabahNumber { get; set; }
        [Required(ErrorMessage = " نام صاحب حساب را وارد کنید")]
        public string OwnerName { get; set; }
        [Required(ErrorMessage = "شماره حساب را وارد کنید")]
        public string AccountNumber { get; set; }
        [Required(ErrorMessage = " نام بانک را وارد کنید")]
        public string BankName { get; set; }
    }
}
