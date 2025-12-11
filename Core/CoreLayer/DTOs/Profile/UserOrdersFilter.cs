using System;
using System.Collections.Generic;
using CoreLayer.DTOs.Pagination;
using DomainLayer.Models.Orders;

namespace CoreLayer.DTOs.Profile
{
    public class UserOrdersFilter:BasePaging
    {
        public List<Order> Orders { get; set; }
        public long UserId { get; set; }
        public long Id { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public OrderStatus Status { get; set; }
    }
}