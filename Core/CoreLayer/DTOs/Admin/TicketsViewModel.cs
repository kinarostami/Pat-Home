using System.Collections.Generic;
using CoreLayer.DTOs.Pagination;
using DomainLayer.Models.Tickets;

namespace CoreLayer.DTOs.Admin
{
    public class AdminTicketsViewModel:BasePaging
    {
        public List<Ticket> Tickets { get; set; }
        public string SearchType { get; set; }
        public int TicketId { get; set; }
    }
}