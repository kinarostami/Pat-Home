using System.ComponentModel.DataAnnotations;

namespace CoreLayer.DTOs.Auth;

public class ResetPasswordDto
{
    public string ActiveCode { get; set; }
    public string Email { get; set; }
    [Display(Name = "کلمه عبور جدید")]
    [Required(ErrorMessage = "{0} نمی تواند خالی باشد")]
    [MinLength(6, ErrorMessage = "{0} باید بیشتر از 6 کاراکتر باشد")]
    public string Password { get; set; }
    [Display(Name = " تکرار کلمه عبور")]
    [Compare("Password", ErrorMessage = "کلمه های عبور یکسان نیستند")]
    public string RePassword { get; set; }
}