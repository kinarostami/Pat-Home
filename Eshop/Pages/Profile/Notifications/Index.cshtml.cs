using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLayer.DTOs.Notifications;
using CoreLayer.Services.Notifications;
using Common.Application.UserUtil;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Pages.Profile.Notifications
{
    [Authorize]
    public class IndexModel : PageUtil
    {
        private readonly INotificationService _notification;

        public IndexModel(INotificationService notification)
        {
            _notification = notification;
        }

        public NotificationsPagination Notifications { get; set; }
        public async Task OnGet(int pageId = 1)
        {
            Notifications = await _notification.GetNotifications(pageId, User.GetUserId(), 13);
        }

        public async Task<IActionResult> OnGetShowNotification(long id)
        {
            var model = await _notification.GetNotification(id);
            if (model == null)
                return NotFound();
            if (model.UserId != User.GetUserId())
                return NotFound();
            if (!model.IsSeen)
                await _notification.SeenNotification(model.Id);
            return Partial("_Show", model);
        }

        public async Task<IActionResult> OnGetDeleteAll()
        {
            return await AjaxTryCatch(async () =>
            {
                await _notification.DeleteAllNotification(User.GetUserId());
            }, successReturn: "Deleted");
        }
    }
}
