using CoreLayer.DTOs.Tickets;
using CoreLayer.Services.Tickets;
using CoreLayer.Utilities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DomainLayer.Models.Roles;
using DomainLayer.Models.Tickets;

namespace Eshop.Areas.Admin.Pages.Tickets
{
    [PermissionChecker(Permissions.مدیریت_تیکت_ها)]
    public class IndexModel : PageModel
    {
        private readonly ITicketService _ticket;

        public IndexModel(ITicketService ticket)
        {
            _ticket = ticket;
        }
        public TicketsFilterDto Tickets { get; set; }
        public async Task OnGet(int pageId = 1, int ticketId = 0, TicketStatus searchType = 0)
        {
            Tickets = await _ticket.GetTickets(pageId, 15, 0, ticketId, null, searchType);
        }
    }
}
