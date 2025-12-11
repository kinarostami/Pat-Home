using System.Collections.Generic;
using CoreLayer.DTOs.Pagination;
using DomainLayer.Models.Users;

namespace CoreLayer.DTOs
{
    public class UserPointsFilter:BasePaging
    {
        public List<UserPoint> Points { get; set; }
        public long UserId { get; set; }
    }
}