using Common.Application.DateUtil;
using CoreLayer.DTOs.Admin;
using CoreLayer.DTOs.Admin.MainPage;
using DataLayer.Context;
using DomainLayer.Models;
using DomainLayer.Models.Articles;
using DomainLayer.Models.Orders;
using DomainLayer.Models.Products;
using DomainLayer.Models.Tickets;
using DomainLayer.Models.Users;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.AdminMainPage;

public interface IAdminPageService
{
    MainPageViewModel GetMainPageService();
    AdminNotificationViewModel GetNotificationsForAdmin();
}
public class AdminPageService : BaseService,IAdminPageService
{
    public AdminPageService(AppDbContext context) : base(context)
    {
        
    }
    public MainPageViewModel GetMainPageService()
    {
        var users = Table<User>();

        var orders = Table<Order>()
            .Include(x => x.User)
            .Include(x => x.Details)
            .Where(x => x.IsFinally);

        var tickets = Table<Ticket>().Include(x => x.User).Where(x => x.Status == TicketStatus.Waiting_For_Reply)
            .OrderByDescending(x => x.CreationDate).ToList();

        return new MainPageViewModel()
        {
            UserCount = users.Count(),
            ProductCount = Table<Product>().Count(x => x.Status == ProductStatus.Active),
            NewOrders = orders.OrderByDescending(x => x.PaymentDate).Where(x => x.Status == OrderStatus.پرداخت_شده).Take(10).ToList(),
            OrdersCount = orders.Sum(x => x.ItemCount),
            NewTickets = tickets,
            RegisterChart = GenerateRegisterChartData(users),
            SalesChart = GenerateSalesChartValues(Table<OrderDetail>()
            .Include(x => x.Order)
            .Where(x => x.Order.Status != OrderStatus.پرداخت_شده))
        };
    }

    public AdminNotificationViewModel GetNotificationsForAdmin()
    {
        var contactUs = Table<ContactUs>().Where(c => !c.IsSeenAdmin).OrderByDescending(d => d.CreationDate);
        var order = Table<Order>().Where(o => o.Status == OrderStatus.پرداخت_شده);
        var tickets = Table<Ticket>().Where(t => t.Status == TicketStatus.Waiting_For_Reply);
        var productComments = Table<ProductComment>().Where(c => c.CreationDate.Date == DateTime.Now.Date);
        var articleComments = Table<ArticleComment>().Where(c => c.CreationDate.Date == DateTime.Now.Date);
        return new AdminNotificationViewModel()
        {
            NotificationCount = (order.Any() ? 1 : 0) +
                                (tickets.Any() ? 1 : 0) +
                                 (articleComments.Any() ? 1 : 0) +
                                 (productComments.Any() ? 1 : 0),
            MessageCount = contactUs.Count(),
            MessageNotifications = GenerateNotification(contactUs),
            OrderNotifications = GenerateNotification(order),
            TicketNotifications = GenerateNotification(tickets),
            ArticleCommentsNotifications = GenerateNotification(articleComments),
            ProductCommentsNotifications = GenerateNotification(productComments),
        };
    }

    #region Notifications
    private string GenerateNotification(IQueryable<ProductComment> entity)
    {
        if (!entity.Any()) return null;

        var message = $"<li><a href='/Admin/products/comments?startDate={DateTime.Now.ToPersianDate()}&endDate={DateTime.Now.ToPersianDate()}'>{entity.Count()} نظر در بخش محصولات ثبت شده است</a></li>";


        return message;
    }
    private string GenerateNotification(IQueryable<ArticleComment> entity)
    {
        if (!entity.Any()) return null;

        var message = $"<li><a href='/Admin/articles/comments?startDate={DateTime.Now.ToPersianDate()}&endDate={DateTime.Now.ToPersianDate()}'>{entity.Count()} نظر در بخش مقالات ثبت شده است</a></li>";


        return message;
    }
    private string GenerateNotification(IQueryable<Ticket> entity)
    {
        if (!entity.Any()) return null;

        var message = $"<li><a href='/Admin/Tickets?searchType=Waiting_For_Reply'>{entity.Count()} تیکت جدید ثبت شده است</a></li>";

        return message;
    }
    private string GenerateNotification(IQueryable<Product> entity)
    {
        if (!entity.Any()) return null;

        //var message = $"<li><a href='/Admin/Products?status=NewRequest'>{entity.Count()} محصول جدید ثبت شده است</a></li>";
        var message = $"<li><a href='/Admin/Products'>{entity.Count()} محصول جدید ثبت شده است</a></li>";

        return message;
    }
    private string GenerateNotification(IQueryable<Order> entity)
    {
        if (!entity.Any()) return null;

        var message = $"<li><a href='/Admin/Orders/Products?status=پرداخت_شده'>{entity.Count()} سفارش جدید ثبت شده است</a></li>";

        return message;
    }
    private string GenerateNotification(IQueryable<ContactUs> messages)
    {
        if (!messages.Any()) return null;
        var message = "";
        foreach (var item in messages)
        {
            message += $"<li><a href='/Admin/ContactUs/Show/{item.Id}'>{item.Subject}</a></li>";
        }
        return message;
    }

    #endregion

    private UserRegisterChartViewModel GenerateRegisterChartData(IQueryable<User> users)
    {
        var registerDailyChart = new UserRegisterChartViewModel();
        var registerPerDay = new List<string>();
        var days = new List<string>();
        var currentDate = DateTime.Now.Date;
        var detailsFiltered =
            users.Where(t =>
                t.CreationDate.Date <= currentDate && t.CreationDate.Date >= currentDate.AddDays(-7));
        for (var i = 0; i > -7; i--)
        {
            var dateFiltered = currentDate.AddDays(i);
            var registerCount = detailsFiltered.Count(u => u.CreationDate.Date == dateFiltered.Date);
            registerPerDay.Add(registerCount.ToString());
            days.Add(dateFiltered.ToPersianDate());
        }
        registerDailyChart.Values = JsonConvert.SerializeObject(registerPerDay);
        registerDailyChart.Days = JsonConvert.SerializeObject(days);
        return registerDailyChart;
    }
    private DailySalesChartViewModel GenerateSalesChartValues(IQueryable<OrderDetail> orderDetails)
    {
        var dailySalesChart = new DailySalesChartViewModel();
        var salesPerDays = new List<string>();
        var days = new List<string>();
        var currentDate = DateTime.Now.Date;
        var detailsFiltered =
            orderDetails.Where(t =>
                t.Order.PaymentDate.Date <= currentDate && t.Order.PaymentDate.Date >= currentDate.AddDays(-7));
        for (var i = 0; i > -7; i--)
        {
            var dateFiltered = currentDate.AddDays(i);
            var detailCount = detailsFiltered.Where(u => u.Order.PaymentDate.Date == dateFiltered.Date).Sum(d => d.Count);
            salesPerDays.Add(detailCount.ToString());
            days.Add(dateFiltered.ToPersianDate());
        }
        dailySalesChart.Values = JsonConvert.SerializeObject(salesPerDays);
        dailySalesChart.Days = JsonConvert.SerializeObject(days);
        return dailySalesChart;
    }
}