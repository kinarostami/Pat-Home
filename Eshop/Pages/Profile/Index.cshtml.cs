using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLayer.Services.Notifications;
using CoreLayer.Services.Users;
using Common.Application.UserUtil;
using DomainLayer.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Pages.Profile
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly INotificationService _notification;

        public IndexModel(IUserService userService, INotificationService notification)
        {
            _userService = userService;
            _notification = notification;
        }
        public User CurrentUser { get; set; }
        public async Task<IActionResult> OnGet()
        {
            CurrentUser = await _userService.GetSingleUser(User.GetUserId());
            return Page();
        }

        public async Task<IActionResult> OnGetHeaderValues()
        {
            var values = await _notification.GeUnSeenNotifications(User.GetUserId());

            return new ObjectResult(new { newNotificationsCount = values.Count, newNotifications = values });
        }
    }
}
