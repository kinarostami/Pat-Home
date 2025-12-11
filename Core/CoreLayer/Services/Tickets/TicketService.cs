using Common.Application.SecurityUtil;
using CoreLayer.DTOs.Tickets;
using CoreLayer.Services.Notifications;
using DataLayer.Context;
using DomainLayer.Models.Tickets;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CoreLayer.Services.Tickets;

public class TicketService : BaseService, ITicketService
{
    private readonly INotificationService _notificationService;
    public TicketService(AppDbContext context, INotificationService notificationService) : base(context)
    {
        _notificationService = notificationService;
    }

    public async Task CloseTicket(long ticketId)
    {
        var ticket = await _context.Tickets.SingleOrDefaultAsync(x => x.Id == ticketId);
        if (ticket == null) throw new InvalidExpressionException();

        ticket.Status = TicketStatus.Close;
        Update(ticket);
        await Save();
    }

    public async Task CreateTicket(Ticket ticket)
    {
        Insert(ticket);
        await Save();
    }

    public async Task<Ticket> GetTicket(long ticketId)
    {
        return await Table<Ticket>().Include(c => c.TicketMessages)
                .ThenInclude(t => t.User).SingleOrDefaultAsync(t => t.Id == ticketId);
    }

    public async Task<Ticket> GetTicket(long ticketId, long userId)
    {
        return await Table<Ticket>().Include(c => c.TicketMessages)
                .ThenInclude(t => t.User).SingleOrDefaultAsync(t => t.Id == ticketId && t.BuilderId == userId);
    }

    public async Task<TicketsFilterDto> GetTickets(int pageId, int take, long userId, long ticketId, string title, TicketStatus status)
    {
        var res = Table<Ticket>().Include(t => t.TicketMessages)
                .Include(t => t.User).AsQueryable();

        if (!string.IsNullOrEmpty(title))
        {
            res = res.Where(r => r.TicketTitle.Contains(title));
        }

        if (userId > 0)
        {
            res = res.Where(r => r.BuilderId == userId);
        }
        if (ticketId > 0)
        {
            res = res.Where(r => r.Id == ticketId);
        }
        if (status > 0)
        {
            res = res.Where(r => r.Status == status);
        }
        var skip = (pageId - 1) * take;
        var model = new TicketsFilterDto()
        {
            Tickets = await res.OrderByDescending(d => d.CreationDate).Skip(skip).Take(take).ToListAsync(),
            Id = ticketId,
            UserId = userId,
            Status = status,
            Title = title
        };
        model.GeneratePaging(res, take, pageId);
        return model;
    }

    public async Task SendMessageToTicket(TicketMessage message)
    {
        var ticket = await _context.Tickets.FindAsync(message.TicketId);
        ticket.Status = TicketStatus.Replied;
        message.MessageBody = message.MessageBody.SanitizeText();
        Update(ticket);
        Insert(message);
        await _notificationService.SendNotificationForTicket(ticket);
    }

    public async Task SendMessageToTicket(TicketMessage message, long userId)
    {
        var ticket = await _context.Tickets.FindAsync(message.TicketId);
        if (ticket.BuilderId != userId) throw new Exception();

        ticket.Status = TicketStatus.Waiting_For_Reply;
        message.MessageBody = message.MessageBody.SanitizeText();

        Update(ticket);
        Insert(message);
        await Save();
    }
}
