using System.ComponentModel.DataAnnotations;

namespace CoreLayer.DTOs.Profile
{
    public class AddWithdrawalViewModel
    {
        public long UserId { get; set; } = 0;

        [Required(ErrorMessage = "مبلغ درخواستی را وارد کنید")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "کارت بانکی را انتخاب کنید")]
        [Range(1,9999999999999999999,ErrorMessage = "کارت بانکی را انتخاب کنید")]
        public long CardId { get; set; }
    }
}