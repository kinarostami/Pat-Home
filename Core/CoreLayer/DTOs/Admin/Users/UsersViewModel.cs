using System.Collections.Generic;
using CoreLayer.DTOs.Pagination;
using DomainLayer.Models.Users;

namespace CoreLayer.DTOs.Admin.Users
{
    public class UsersViewModel:BasePaging
    {
        public List<User> Users { get; set; }
        public string Email { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}