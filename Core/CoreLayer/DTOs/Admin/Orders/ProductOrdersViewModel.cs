using System;
using System.Collections.Generic;
using CoreLayer.DTOs.Pagination;
using DomainLayer.Models.Orders;

namespace CoreLayer.DTOs.Admin.Orders
{
    public class ProductOrdersViewModel:BasePaging
    {
        public List<Order> ProductOrders { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long OrderId { get; set; }
        public long UserId { get; set; }
        public string Shire { get; set; }
        public string City { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}