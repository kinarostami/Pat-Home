using System.Collections.Generic;
using DomainLayer.Models.Orders;
using DomainLayer.Models.Tickets;

namespace CoreLayer.DTOs.Admin.MainPage
{
    public class MainPageViewModel
    {
        public int UserCount { get; set; }
        public int OrdersCount { get; set; }
        public int ProductCount { get; set; }
        public UserRegisterChartViewModel RegisterChart { get; set; }
        public DailySalesChartViewModel SalesChart { get; set; }
        public List<Ticket> NewTickets { get; set; }
        public List<Order> NewOrders { get; set; }
    }
}