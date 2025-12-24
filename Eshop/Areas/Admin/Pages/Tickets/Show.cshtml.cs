using CoreLayer.Services;
using CoreLayer.Services.Tickets;
using CoreLayer.Utilities;
using Common.Application.UserUtil;
using DomainLayer.Models.Tickets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages.Tickets
{
    [PermissionChecker(Permissions.مدیریت_تیکت_ها)]

    [ValidateAntiForgeryToken]
    public class ShowModel : PageModel
    {
        private readonly ITicketService _ticket;
        private IAppContext _appContext;

        public ShowModel(ITicketService ticket, IAppContext appContext)
        {
            _ticket = ticket;
            _appContext = appContext;
        }
    
      
        public Ticket TicketModel { get; set; }
        public async Task<IActionResult> OnGetAsync(long ticketId)
        {
            var ticket = await _ticket.GetTicket(ticketId);
            if (ticket == null)
            {
                return RedirectToPage("Index");
            }
            TicketModel = ticket;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(long ticketId, string message)
        {
          
            if (string.IsNullOrEmpty(message))
            {
                return Page();
            }
            var ticketMessage = new TicketMessage()
            {
                MessageBody = message,
                TicketId = ticketId,
                UserId = User.GetUserId()
            };
            try
            {
                await _ticket.SendMessageToTicket(ticketMessage);
                TempData["Success"] = true;
                return Redirect("/Admin/Tickets/Show/" + ticketId);
            }
            catch
            {
                TempData["Error"] = false;
                return Redirect("/Admin/Tickets/Show/" + ticketId);
            }
        }

        public async Task<IActionResult> OnGetCloseTicket(int ticketId)
        {
            try
            {
                await _ticket.CloseTicket(ticketId);
                return Content("Success");
            }
            catch
            {
                return Content("Error");
            }
        }
    }
}
