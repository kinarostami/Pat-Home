using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.DTOs.Auth;

public class LoginDto
{
    public string ReturnUrl { get; set; }
    [Display(Name = "شماره تلفن")]
    [Required(ErrorMessage = "{0} نمی تواند خالی باشد")]
    [MaxLength(11, ErrorMessage = "{0} نامعتبر است")]
    [MinLength(11, ErrorMessage = "{0} نامعتبر است")]
    public string PhoneNumber { get; set; }
    [Display(Name = "کلمه عبور")]
    [Required(ErrorMessage = "{0} نمی تواند خالی باشد")]
    public string Password { get; set; }
}
