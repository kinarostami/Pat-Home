using CoreLayer.DTOs.Profile;
using CoreLayer.DTOs.Shop;
using CoreLayer.Services.Emails;
using CoreLayer.Services.Notifications;
using CoreLayer.Services.Wallets;
using DataLayer.Context;
using DomainLayer.Models.Orders;
using DomainLayer.Models.Products;
using DomainLayer.Models.Users;
using DomainLayer.Models.Wallets;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Orders;

public class OrderService : BaseService, IOrderService
{
    private readonly IEmailService _email;
    private readonly IWalletService _wallet;
    private readonly INotificationService _notification;
    public OrderService(AppDbContext context, IEmailService email, IWalletService wallet, INotificationService notification) : base(context)
    {
        _email = email;
        _wallet = wallet;
        _notification = notification;
    }

    public async Task<Tuple<AddToShopCart, int>> AddToShopCart(AddProductToCartDto productModel)
    {
        var inventory = await _inventory.GetById(productModel.InventoryId);
        if (inventory == null)
            return Tuple.Create(CoreLayer.Services.Orders.AddToShopCart.Error, 0);

        if (inventory.Count < productModel.Count)
            return Tuple.Create(CoreLayer.Services.Orders.AddToShopCart.Count_IS_Not_Enough, 0);

        var product = await GetById<Product>(inventory.ProductId);
        var order = await GetCurrentOrder(productModel.UserId);
        var price = CalculatePrice(inventory.Price, product.DiscountPercentage);
        if (order != null)
        {
            var detailSelected = order.Details.FirstOrDefault(d => d.InventoryId == inventory.Id);
            if (detailSelected != null)
            {

                if (inventory.Count < productModel.Count + detailSelected.Count)
                    return Tuple.Create(CoreLayer.Services.Orders.AddToShopCart.Count_IS_Not_Enough, order.ItemCount);

                detailSelected.Count += productModel.Count;
                detailSelected.Price = price;
                Update(detailSelected);
            }
            else
            {
                var detail = new OrderDetail()
                {
                    Count = productModel.Count,
                    OrderId = order.Id,
                    ProductId = inventory.ProductId,
                    Price = price,
                    CreationDate = DateTime.Now,
                    StackType = productModel.StackType,
                    InventoryId = inventory.Id
                };
                Insert(detail);
                order.Details.Add(detail);
            }

            order.ItemCount += productModel.Count;
            order.Price = order.Details.Sum(s => s.TotalPrice);
            await Save();
            return Tuple.Create(CoreLayer.Services.Orders.AddToShopCart.Success, order.ItemCount);
        }

        #region Create New Order

        var currentOrder = new Order()
        {
            ByWallet = false,
            DiscountTitle = null,
            Discount = null,
            IsFinally = false,
            UserId = productModel.UserId,
            Status = OrderStatus.در_انتظار_پرداخت,
            SendTypeTitle = "",
            IsSendFactor = false,
            ItemCount = productModel.Count,
            Details = new List<OrderDetail>()
                {
                    new OrderDetail()
                    {
                        Count = productModel.Count,
                        Price = price,
                        ProductId = inventory.ProductId,
                        CreationDate = DateTime.Now,
                        StackType = productModel.StackType,
                        InventoryId = inventory.Id
                    }
                }
        };
        currentOrder.Price = currentOrder.Details.Sum(s => s.TotalPrice);
        Insert(currentOrder);
        await Save();
        return Tuple.Create(CoreLayer.Services.Orders.AddToShopCart.Success, productModel.Count);

        #endregion
    }

    public async Task ChangeDetailCount(long userId, int count, long detailId)
    {
        var detail = await TableTracking<OrderDetail>()
                .Include(c => c.Order)
                .SingleOrDefaultAsync(d => d.Id == detailId);

        var inventory = await _inventory.GetById(detail.InventoryId);

        if (detail == null)
            throw new Exception();

        if (detail.Order.UserId != userId)
            throw new Exception();

        if (count > inventory.Count || count <= 0)
            throw new Exception();

        detail.Count = count;
        var order = TableTracking<Order>()
                .Include(c => c.Details).First(d => d.Id == detail.OrderId);

        order.Price = order.Details.Sum(p => p.TotalPrice);
        order.ItemCount = order.Details.Sum(p => p.Count);
        await Save();
    }

    public async Task ChangeStatusToReceived(long orderId, long userId)
    {
        var order = await GetById<Order>(orderId);
        if (order == null)
            throw new Exception();
        if (order.UserId != userId)
            throw new Exception();
        order.Status = OrderStatus.تحویل_به_مشتری;
        Update(order);
        await Save();
    }

    public async Task<Tuple<Order, bool>> CheckOrderAndReturn(long userId)
    {
        var isChanged = false;
        var currentOrder = await TableTracking<Order>()
            .Include(c => c.Details)
            .ThenInclude(c => c.Product)
            .Include(c => c.Address)
            .SingleOrDefaultAsync(o => o.UserId == userId && !o.IsFinally);

        if (currentOrder == null)
            return Tuple.Create(currentOrder, false);

        if (!currentOrder.Details.Any())
        {
            currentOrder.Discount = null;
            currentOrder.DiscountPercentage = null;
            currentOrder.DiscountTitle = null;
            Update(currentOrder);
            await Save();
            return Tuple.Create(currentOrder, false);
        }

        foreach (var detail in currentOrder.Details)
        {

            var inventory = await _inventory.GetById(detail.InventoryId);

            if (inventory == null)
            {
                isChanged = true;
                currentOrder.Details.Remove(detail);
            }
            else
            {
                var product = await GetById<Product>(inventory.ProductId);
                var inventoryPrice = CalculatePrice(inventory.Price, product.DiscountPercentage);
                if (inventory.Count < detail.Count)
                {
                    isChanged = true;
                    if (inventory.Count == 0)
                    {
                        currentOrder.Details.Remove(detail);
                    }
                    else
                    {
                        detail.Count = inventory.Count;
                    }
                }

                if (detail.Price == inventoryPrice) continue;

                isChanged = true;
                detail.Price = inventoryPrice;
            }
        }
        currentOrder.Price = currentOrder.Details.Sum(d => d.TotalPrice);
        currentOrder.ItemCount = currentOrder.Details.Sum(d => d.Count);
        Update(currentOrder);
        await Save();
        return Tuple.Create(currentOrder, isChanged);
    }

    public async Task ClearOrdersDiscount()
    {
        var orders = Table<Order>()
                .Where(o => !o.IsFinally && (string.IsNullOrWhiteSpace(o.DiscountTitle))
                                         && o.CreationDate.Date != DateTime.Now.Date);

        foreach (var order in orders)
        {
            order.DiscountTitle = null;
            order.DiscountPercentage = null;
            order.Discount = null;
            Update(order);
        }

        if (orders.Any())
            await Save();
    }

    public async Task DeleteOrderDetail(long detailId)
    {
        var detail = await TableTracking<OrderDetail>()
                .Include(c => c.Order)
                .ThenInclude(c => c.Details)
                .SingleOrDefaultAsync(d => d.Id == detailId);

        if (detail == null)
            throw new Exception();

        var order = await TableTracking<Order>()
            .Include(c => c.Details)
            .FirstAsync(d => d.Id == detail.OrderId);

        #region Delete OrderDetail

        Delete(detail);
        order.Price -= detail.TotalPrice;
        order.ItemCount -= detail.Count;
        Update(order);
        await Save();

        #endregion
    }

    public async Task DeleteOrderDetail(long detailId, long userId)
    {
        var detail = await TableTracking<OrderDetail>()
                .Include(c => c.Order)
                .SingleOrDefaultAsync(d => d.Id == detailId);

        if (detail == null || detail.Order.UserId != userId)
            throw new Exception("خظایی رخ داده است");

        var order = await TableTracking<Order>()
            .Include(c => c.Details)
            .FirstAsync(d => d.Id == detail.OrderId);

        #region Delete OrderDetail

        Delete(detail);
        order.Price -= detail.TotalPrice;
        order.ItemCount -= detail.Count;
        Update(order);
        await Save();

        #endregion
    }

    public async Task FinallyOrder(Order order)
    {
        if (order.WalletAmount > 0)
        {
            var userWalletAmount = await _wallet.BalanceWallet(order.UserId);
            if (userWalletAmount < order.WalletAmount)
                throw new InvalidExpressionException("موجودی حساب شما کمتر از مقدار فاکتور می باشد");
        }

        order.ByWallet = order.ByWallet;
        order.IsFinally = true;
        order.Status = OrderStatus.پرداخت_شده;
        order.PaymentDate = DateTime.Now;
        ManageWallet(order);
        //await DecreaseInventory(order);
        AddPointForUser(order);
        await Save();
        try
        {
            await _notification.SendNotificationForFinallyOrder(order);
        }
        catch
        {
            //Ignored 
        }
    }

    public async Task<Order> GetCurrentOrder(long userId)
    {
        return await TableTracking<Order>()
                .Include(c => c.User)
                .Include(c => c.Details)
                .ThenInclude(c => c.Product)
                .OrderBy(c => c.Id)
                .SingleOrDefaultAsync(o => o.UserId == userId && !o.IsFinally);
    }

    public async Task<Order> GetOrderById(long id)
    {
        return await TableTracking<Order>()
                .Include(c => c.User)
                .Include(c => c.Address)
                .Include(c => c.Details)
                .ThenInclude(c => c.Product)
                .SingleOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Order> GetOrderById(long id, long userId)
    {
        return await Table<Order>()
                .Include(c => c.User)
                .Include(c => c.Address)
                .Include(c => c.Details)
                .ThenInclude(c => c.Product)
                .SingleOrDefaultAsync(o => o.Id == id && o.UserId == userId);
    }

    public async Task<UserOrdersFilter> GetOrders(int pageId, int take, long userId, long id, OrderStatus status, string phoneNumber, DateTime? startDate, DateTime? endDate)
    {
        var res = Table<Order>()
                .Include(c => c.Address)
                .Include(c => c.User)
                .Include(c => c.Details)
                .Where(o => o.Status != OrderStatus.در_انتظار_پرداخت);

        if (!string.IsNullOrEmpty(phoneNumber))
        {
            res = res.Where(r => r.User.PhoneNumber.Contains(phoneNumber));
        }
        if (userId >= 1)
        {
            res = res.Where(r => r.UserId == userId);
        }
        if (id >= 1)
        {
            res = res.Where(r => r.Id == id);
        }
        if (status > 0)
        {
            res = res.Where(r => r.Status == status);
        }
        if (startDate != null)
        {
            res = res.Where(r => r.PaymentDate.Date >= startDate.Value.Date);
        }
        if (endDate != null)
        {
            res = res.Where(r => r.PaymentDate.Date <= endDate.Value.Date);
        }
        var skip = (pageId - 1) * take;
        var model = new UserOrdersFilter()
        {
            Orders = await res.OrderByDescending(d => d.PaymentDate).Skip(skip).Take(take).ToListAsync(),
            UserId = userId,
            Id = id,
            Status = status,
            PhoneNumber = phoneNumber,
            StartDate = startDate,
            EndDate = endDate
        };

        model.GeneratePaging(res, take, pageId);
        return model;
    }

    public async Task<int> GetShopCartItemCount(long userId)
    {
        var order = await Table<Order>().FirstOrDefaultAsync(o => o.UserId == userId && !o.IsFinally);
        if (order == null)
            return 0;

        return order.ItemCount;
    }

    public async Task<UserOrdersFilter> GetUserOrders(int pageId, int take, long userId)
    {
        var res = Table<Order>()
                .Include(c => c.Address)
                .Include(c => c.User)
                .Include(c => c.Details).Where(r => r.UserId == userId);

        var skip = (pageId - 1) * take;
        var model = new UserOrdersFilter()
        {
            Orders = await res.OrderByDescending(d => d.PaymentDate).Skip(skip).Take(take).ToListAsync(),
            UserId = userId,
        };

        model.GeneratePaging(res, take, pageId);
        return model;
    }

    public async Task SendProduct(Order order, string tackingCode)
    {
        if (order == null) throw new Exception();

        order.TrackingCode = tackingCode;
        order.IsSend = true;
        order.SendDate = DateTime.Now;
        order.Status = OrderStatus.ارسال_شده;
        Update(order);
        var notification = new UserNotification()
        {
            IsSeen = false,
            UserId = order.UserId,
            NotificationTitle = $"ارسال محصولات سفارش #{order.Id}",
            NotificationBody = $"{order.User.Name} {order.User.Family} عزیر محصولات فاکتور #{order.Id} برای شما ارسال شد برای نمایش جزئیات روی <a href='/Profile/Orders/Show/{order.Id}'>این لینک</a> کلیک کنید."
        };
        Insert(notification);
        await Save();
        try
        {
            _email.SendTrackingCode(order);
        }
        catch
        {
            //ignored
        }
    }

    public async Task UpdateOrder(Order order)
    {
        Update(order);
        await Save();
    }

    public async Task UpdateOrder(Order order, long addressId)
    {
        var address = await GetById<UserAddress>(addressId);
        if (address == null)
            throw new Exception("لطفا یک آدرس انتخاب کنید");

        if (address.UserId != order.UserId)
            throw new Exception();

        var orderAddresses = TableTracking<OrderAddress>().Where(a => a.OrderId == order.Id);
        if (orderAddresses.Any())
        {
            Delete(orderAddresses);
        }
        var orderAddress = new OrderAddress()
        {
            City = address.City,
            Address = address.Address,
            Family = address.Family,
            Shire = address.Shire,
            PostalCode = address.PostalCode,
            Phone = address.Phone,
            OrderId = order.Id,
            Name = address.Name,
            NationalCode = address.NationalCode
        };
        Insert(orderAddress);
        order.SendTypeTitle = "پست";
        Update(order);
        await Save();
    }

    #region Utils

    private void AddPointForUser(Order order)
    {
        var kiloGram = order.Details.Sum(d => d.KiloGram);

        var point = kiloGram / 1000;
        var userPoint = new UserPoint()
        {
            Count = point,
            CreationDate = DateTime.Now,
            Description = $"امتیاز شما از خرید {point} کیلو عسل",
            Type = WalletType.واریز,
            UserId = order.UserId
        };
        Insert(userPoint);
    }
    //private async Task DecreaseInventory(Order order)
    //{
    //    foreach (var detail in order.Details)
    //    {
    //        await _inventory.DeCreaseInventoryWithoutSave(new DecreaseInventoryCommand()
    //        {
    //            Count = detail.Count,
    //            Id = detail.InventoryId
    //        });
    //    }

    //    await _inventory.SaveChange();
    //}
    private void ManageWallet(Order order)
    {
        if (order.WalletAmount > 0)
        {
            Insert(new Wallet()
            {
                Amount = order.WalletAmount,
                Description = $"پرداخت فاکتور {order.Id}",
                IsFinally = true,
                WalletType = WalletType.برداشت,
                UserId = order.UserId,
                PaymentDate = DateTime.Now
            });
        }

    }
    private int CalculatePrice(int price, int discountPercentage)
    {
        var finalPrice = price;
        if (discountPercentage > 0)
        {
            var discount = discountPercentage * price / 100;
            finalPrice -= discount;
        }
        return finalPrice;
    }
    #endregion
}
