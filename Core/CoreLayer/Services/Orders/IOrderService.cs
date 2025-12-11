using CoreLayer.DTOs.Profile;
using CoreLayer.DTOs.Shop;
using DomainLayer.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Orders;

public interface IOrderService
{
    Task<Tuple<AddToShopCart, int>> AddToShopCart(AddProductToCartDto productModel);
    Task<UserOrdersFilter> GetOrders(int pageId, int take, long userId, long id, OrderStatus status, string phoneNumber, DateTime? startDate, DateTime? endDate);
    Task<UserOrdersFilter> GetUserOrders(int pageId, int take, long userId);
    Task<Order> GetCurrentOrder(long userId);
    Task<Order> GetOrderById(long id);
    Task<Order> GetOrderById(long id,long userId);
    Task<int> GetShopCartItemCount(long userId);
    Task ChangeDetailCount(long userId,int count,long detailId);
    Task<Tuple<Order, bool>> CheckOrderAndReturn(long userId);
    Task DeleteOrderDetail(long detailId);
    Task DeleteOrderDetail(long detailId, long userId);
    Task UpdateOrder(Order order);
    Task UpdateOrder(Order order, long addressId);
    Task FinallyOrder(Order order);
    Task SendProduct(Order order, string tackingCode);
    Task ChangeStatusToReceived(long orderId, long userId);
    Task ClearOrdersDiscount();
}
    public enum AddToShopCart
    {
        Success,
        Error,
        Count_IS_Not_Enough
    }

