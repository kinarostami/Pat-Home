using System;
using System.ComponentModel.DataAnnotations;
using DomainLayer.Models.Users;
using Microsoft.AspNetCore.Http;

namespace CoreLayer.DTOs.Profile
{
    public class EditProfileDto
    {
        public long UserId { get; set; } = 0;
        [Display(Name = "نام")]
        [Required(ErrorMessage = "{0} خود را وارد کنید")]
        public string Name { get; set; }
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "{0}  را وارد کنید")]
        public string Family { get; set; }
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        [Display(Name = "شماره تلفن")]
        public string PhoneNumber { get; set; }
        [Display(Name = "کد ملی")]
        [MaxLength(10, ErrorMessage = "{0} نامعتبر است")]
        [MinLength(10, ErrorMessage = "{0} نامعتبر است")]
        public string NationalCode { get; set; }
        public string ImageName { get; set; }
        [Display(Name = "جنسیت")]
        public UserGender Gender { get; set; } = UserGender.نا_مشخص;
        public IFormFile ImageFile { get; set; }
        [Display(Name = "تاریخ تولد")]
        public DateTime? BirthDate { get; set; }

        public bool IsCompleteUProfile { get; set; }
        public string OldEmail { get; set; }
    }
}