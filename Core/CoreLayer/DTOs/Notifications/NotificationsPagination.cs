using System.Collections.Generic;
using CoreLayer.DTOs.Pagination;
using DomainLayer.Models.Users;

namespace CoreLayer.DTOs.Notifications
{
    public class NotificationsPagination:BasePaging
    {
        public List<UserNotification> Notifications { get; set; }
        public long UserId { get; set; }
    }
}