using System.ComponentModel.DataAnnotations;

namespace CoreLayer.DTOs.Auth;

public class RegisterDto
{
    [Display(Name = "شماره تلفن")]
    [Required(ErrorMessage = "{0} نمی تواند خالی باشد")]
    [MaxLength(11, ErrorMessage = "{0} نامعتبر است")]
    [MinLength(11, ErrorMessage = "{0} نامعتبر است")]
    public string PhoneNumber { get; set; }
    [Display(Name = "کلمه عبور")]
    [Required(ErrorMessage = "{0} نمی تواند خالی باشد")]
    [MinLength(6, ErrorMessage = "{0} باید بیشتر از 6 کاراکتر باشد")]
    public string Password { get; set; }
    [Display(Name = "تکرار کلمه عبور")]
    [Compare("Password", ErrorMessage = "کلمه های عبور یکسان نیستند")]
    public string RePassword { get; set; }
    [MaxLength(11, ErrorMessage = "{0} نامعتبر است")]
    [MinLength(11, ErrorMessage = "{0} نامعتبر است")]
    public string Presenter { get; set; }
}
