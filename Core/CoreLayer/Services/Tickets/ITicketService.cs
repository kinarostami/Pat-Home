using CoreLayer.DTOs.Tickets;
using DomainLayer.Models.Tickets;
using DomainLayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Tickets;

public interface ITicketService
{
    Task<TicketsFilterDto> GetTickets(int pageId,int take,long userId,long ticketId,string title,TicketStatus status);
    Task<Ticket> GetTicket(long ticketId);
    Task<Ticket> GetTicket(long ticketId,long userId);

    Task CreateTicket(Ticket ticket);
    Task SendMessageToTicket(TicketMessage message);
    Task SendMessageToTicket(TicketMessage message,long userId);
    Task CloseTicket(long ticketId);
}
