using CoreLayer.DTOs.Notifications;
using DataLayer.Context;
using DomainLayer.Models.Orders;
using DomainLayer.Models.Tickets;
using DomainLayer.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.Services.Notifications;

public class NotificationService : BaseService, INotificationService
{
    public NotificationService(AppDbContext context) : base(context)
    {
    }

    public async Task DeleteAllNotification(long userId)
    {
        var notification = Table<UserNotification>().Where(x => x.UserId == userId);
        Delete(notification);
        await Save();
    }

    public async Task<UserNotification> GetNotification(long id)
    {
        return await GetById<UserNotification>(id);
    }

    public async Task<NotificationsPagination> GetNotifications(int pageId, long userId, int take)
    {
        var res = Table<UserNotification>().Where(n => n.UserId == userId);
        var skip = (pageId - 1) * take;
        var model = new NotificationsPagination()
        {
            Notifications = await res
                .OrderByDescending(d => d.CreationDate).Skip(skip)
                .Take(take).ToListAsync(),
            UserId = userId
        };
        model.GeneratePaging(res, take, pageId);
        return model;
    }

    public async Task<List<UserNotification>> GeUnSeenNotifications(long userId)
    {
        return await Table<UserNotification>().Where(x => x.UserId == userId && !x.IsSeen).ToListAsync();
    }

    public async Task InsertNotification(UserNotification notification)
    {
        Insert(notification);
        await Save();
    }

    public async Task SeenNotification(long id)
    {
        var notification = await GetNotification(id);
        if (notification != null && !notification.IsSeen)
        {
            notification.IsSeen = true;
            Update(notification);
            await Save();
        }
    }

    public async Task SendNotificationForFinallyOrder(Order order)
    {
        Insert(new UserNotification()
        {
            IsSeen = false,
            NotificationTitle = $"سفارش شما ثبت شده و درحال پردازش است",
            NotificationBody = $"با سلام سفارش شما با موفقیت ثبت شده و بعد از پردازش برای شما ارسال میشود.<p>شما می توانید از طریق <a target='_blank' href='/Profile/Orders/Show/{order.Id}'>این اینک</a> وضعیت سفارش خود را مشاهده کنید.</p>",
            UserId = order.UserId
        });

        await Save();
    }

    public async Task SendNotificationForTicket(Ticket ticket)
    {
        try
        {
            var fullName = ticket.User.Name + " " + ticket.User.Family;
            Insert(new UserNotification()
            {
                IsSeen = false,
                NotificationTitle = $"پاسخ جدیدی در تیکت #{ticket.Id} ثبت شد.",
                NotificationBody = $"{fullName} عزیز برای مشاهده تیکت روی <a href='/Profile/Tickets/show/{ticket.Id}'>این لینک</a> کلیک کنید.",
                UserId = ticket.BuilderId,
            });
        }
        catch
        {
            //ignore
        }
        await Save();
    }
}