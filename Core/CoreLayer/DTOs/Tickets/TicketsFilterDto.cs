using System.Collections.Generic;
using CoreLayer.DTOs.Pagination;
using DomainLayer.Models.Tickets;

namespace CoreLayer.DTOs.Tickets
{
    public class TicketsFilterDto : BasePaging
    {
        public List<Ticket> Tickets { get; set; }
        public long UserId { get; set; } = 0;
        public long Id { get; set; } = 0;
        public string Title { get; set; }
        public TicketStatus Status { get; set; } = 0;

    }
}