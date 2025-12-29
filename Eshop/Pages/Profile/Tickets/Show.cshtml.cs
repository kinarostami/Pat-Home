using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLayer.Services.Tickets;
using Common.Application.UserUtil;
using DomainLayer.Models.Tickets;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Pages.Profile.Tickets
{
    [ValidateAntiForgeryToken]
    [Authorize]
    public class ShowModel : PageUtil
    {
        private readonly ITicketService _ticketService;

        public ShowModel(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        public Ticket Ticket { get; set; }
        public async Task<IActionResult> OnGet(long ticketId)
        {
            Ticket = await _ticketService.GetTicket(ticketId, User.GetUserId());
            if (Ticket == null)
            {
                TempData["Error"] = ResultModel.Error(null);
                return RedirectToPage("Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost(long ticketId, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                Ticket = await _ticketService.GetTicket(ticketId, User.GetUserId());
                TempData["Error"] = ResultModel.Error("متن پیام رو وارد کنید");
                return Page();
            }
            return await TryCatch(async () =>
            {
                await _ticketService.SendMessageToTicket(new TicketMessage()
                {
                    CreationDate = DateTime.Now,
                    MessageBody = message,
                    TicketId = ticketId,
                    UserId = User.GetUserId()
                }, User.GetUserId());
            }, successReturn: $"/Profile/Tickets/Show/{ticketId}", errorReturn: $"/Profile/Tickets/Show/{ticketId}");
        }
    }
}
