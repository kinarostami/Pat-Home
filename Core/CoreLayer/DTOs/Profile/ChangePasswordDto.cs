using System.ComponentModel.DataAnnotations;

namespace CoreLayer.DTOs.Profile
{
    public class ChangePasswordDto
    {
        public long UserId { get; set; }
        [Display(Name = "کلمه عبور فعلی")]
        [Required(ErrorMessage = "{0} نمی تواند خالی باشد")]
        public string CurrentPassword { get; set; }
        [Display(Name = "کلمه عبور جدید")]
        [Required(ErrorMessage = "{0} نمی تواند خالی باشد")]
        [MinLength(6, ErrorMessage = "{0} باید بیشتر از 6 کاراکتر باشد")]
        public string NewPassword { get; set; }
        [Display(Name = " تکرار کلمه عبور")]
        [Compare("NewPassword", ErrorMessage = "کلمه های عبور یکسان نیستند")]
        public string ConfirmPassword { get; set; }
    }
}