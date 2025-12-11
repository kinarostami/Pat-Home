using CoreLayer.DTOs.Notifications;
using DomainLayer.Models.Orders;
using DomainLayer.Models.Tickets;
using DomainLayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Notifications;

public interface INotificationService
{
    Task<NotificationsPagination> GetNotifications(int pageId, long userId, int take);
    Task<UserNotification> GetNotification(long id);
    Task<List<UserNotification>> GeUnSeenNotifications(long userId);
    Task InsertNotification(UserNotification notification);
    Task DeleteAllNotification(long userId);
    Task SeenNotification(long id);
    ///////////////////////////////////////
    Task SendNotificationForTicket(Ticket ticket);
    ///////////////////////////////////
    Task SendNotificationForFinallyOrder(Order order);
}
