using CoreLayer.DTOs.Tickets;
using CoreLayer.Services.Tickets;
using Common.Application.UserUtil;
using DomainLayer.Models.Tickets;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Pages.Profile.Tickets
{
    [Authorize]
    [ValidateAntiForgeryToken]
    public class IndexModel : PageUtil
    {
        private readonly ITicketService _ticketService;

        public IndexModel(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        public TicketsFilterDto TicketsFilter { get; set; }
        public async Task OnGet(int pageId = 1)
        {
            TicketsFilter = await _ticketService.GetTickets(pageId, 8, User.GetUserId(), 0, null, 0);
        }

        public async Task<IActionResult> OnPost(string text, string title)
        {
            var ticket = new Ticket()
            {
                BuilderId = User.GetUserId(),
                CreationDate = DateTime.Now,
                Status = TicketStatus.Waiting_For_Reply,
                TicketBody = text,
                TicketTitle = title
            };
            return await TryCatch(async () =>
            {
                await _ticketService.CreateTicket(ticket);

            }, successReturn: "/Profile/Tickets", errorReturn: "/Profile/Tickets");
        }
    }
}
