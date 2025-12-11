using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CoreLayer.DTOs.Admin.Users
{
    public class EditUserViewModel
    {
        public long Id { get; set; } = 0;
        [Required(ErrorMessage = ("نام خود را وارد کند"))]
        public string Name { get; set; }
        [Required(ErrorMessage = ("نام خانوادگی وارد کند"))]
        public string Family { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImageName { get; set; }
        public string NewPassword { get; set; }
        public bool IsActive { get; set; }
        public List<long> UserRoles { get; set; }
        public IFormFile UserImage { get; set; }
    }
}