using CookieManager;
using CoreLayer.DTOs.Shop;
using CoreLayer.Services.Orders;
using CoreLayer.Services.Products;
using DomainLayer.Models.Orders;
using DomainLayer.Models.Users;
using InventoryManagement.Application.ApplicationSercices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Eshop.Infrastructure
{
    public static class CookieJobs
    {
        public static async Task SaveOrderCookieInDataBase(HttpContext httpContext, IOrderService _order, ICookieManager _cookie, long userId)
        {
            var value = httpContext.Request.Cookies["ShopCart"];
            if (value != null)
            {
                //تمام محصولات سبد خرید را در بانک ذخیره میکنیم
                try
                {
                    var shopCartValue = _cookie.Get<List<AddProductToCartDto>>("ShopCart");
                    foreach (var item in shopCartValue)
                    {
                        item.UserId = userId;
                        await _order.AddToShopCart(item);
                    }

                    httpContext.Response.Cookies.Delete("ShopCart");
                }
                catch
                {
                    //ignor
                }


            }
        }
        public static void SetUserCookie(HttpContext httpContext, User user)
        {
            var userInfo = new 
            {
                name = user.Name,
                family = user.Family,
                imageName = user.ImageName,
                phone = user.PhoneNumber
            };
            httpContext.Response.Cookies.Append(
                "userInfo",
                JsonConvert.SerializeObject(userInfo),
                new CookieOptions()
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                });

        }
        public static Order GenerateFakeOrder(ICookieManager _cookie, IInventoryService _inventory,IProductService productService)
        {
            //برای نمایش دادن فاکتور باید یک فاکتور فرضی بسازیم و مقادیر را در آن ذخیره کنیم 
            //بعد از لاگین کردن یک فاکنور واقعی برای کاربر می سازیم 
            var shopCart = _cookie.Get<List<AddProductToCartDto>>("ShopCart");
            //یک فاکتور فرضی میسازم
            var order = new Order()
            {
                IsFinally = false,
                ItemCount = 0,
                UserId = 1,
                IsSendFactor = false,
                PaymentDate = DateTime.Now,
                Status = OrderStatus.در_انتظار_پرداخت,
                SendTypeTitle = "انتخاب کنید",
                Price = 0,
                ByWallet = false,
                Id = 0
            };
            List<OrderDetail> details = new List<OrderDetail>();
            //اگر کوکی خالی نبود وارد میشه
            if (shopCart != null)
            {
                //روی محصولات داخل کوکی حلقه می زنیم و اونا رو تبدیل به ریز فاکتور میکنیم
                foreach (var item in shopCart)
                {
                    var inventory = _inventory.GetById(item.InventoryId).Result;
                    if (inventory == null)
                    {
                        DeleteItemFromShopCart(_cookie, item.Id);
                        continue;
                    }
                    var detail = new OrderDetail()
                    {
                        Price = inventory.Price,
                        Count = item.Count,
                        Id = item.Id,
                        ProductId = inventory.ProductId,
                        Product = productService.GetProductById(inventory.ProductId).Result,
                        OrderId = order.Id,
                        StackType = item.StackType
                    };
                    details.Add(detail);
                }
            }

            //فاکتور پیشفرض را بروز رسانی میکنیم
            order.ItemCount = details.Sum(c => c.Count);
            order.Price = details.Sum(s => s.TotalPrice);
            order.Details = details;
            return order;
        }

        public static ObjectResult AddProductToShopCart(AddProductToCartDto productModel, HttpRequest request, ICookieManager _cookie)
        {
            Random _random = new Random();
            var number = _random.Next(0, 10000) * 6 ^ 2 + _random.Next(6, 1000000);
            productModel.Id = number;
            if (request.Cookies["ShopCart"] != null)
            {
                //مقدار کوکی را دریافت میکنیم
                var shopCart = _cookie.Get<List<AddProductToCartDto>>("ShopCart");

                //اگر محصول وارد شده قبلا داخل سبد خرید بوده باشه وارد شرط میشه
                var currentProduct = shopCart.SingleOrDefault(s => s.InventoryId == productModel.InventoryId);
                if (currentProduct != null)
                {
                    currentProduct.Count += productModel.Count;
                    //محصولی که باید بروز رسانی کنیم را در لیست پیدا میکنیم 
                    var index = shopCart.FindIndex(s => s.InventoryId == currentProduct.InventoryId);
                    shopCart[index] = currentProduct;

                    _cookie.Set("ShopCart", shopCart, new CookieOptions() { Expires = DateTimeOffset.Now.AddDays(10) });
                    return new ObjectResult(new { message = "success", count = shopCart.Sum(d => d.Count) });
                }
                shopCart.Add(productModel);
                //کوکی را بروز رسانی میکنیم
                _cookie.Set("ShopCart", shopCart, new CookieOptions() { Expires = DateTimeOffset.Now.AddDays(10) });
                return new ObjectResult(new { message = "success", count = shopCart.Sum(d => d.Count) });
            }

            var newProduct = new List<AddProductToCartDto>();
            newProduct.Add(productModel);
            _cookie.Set("ShopCart", newProduct, new CookieOptions() { Expires = DateTimeOffset.Now.AddDays(10) });
            return new ObjectResult(new { message = "success", count = newProduct.Sum(p => p.Count) });
        }

        public static void DeleteItemFromShopCart(ICookieManager _cookie, long id)
        {
            var shopCart = _cookie.Get<List<AddProductToCartDto>>("ShopCart");

            var index = shopCart.FindIndex(s => s.Id == id);
            //-1 = Not Found
            if (index == -1)
                throw new Exception();

            //محصول را از کوکی حذف می کنیم 
            shopCart.RemoveAt(index);
            _cookie.Set("ShopCart", shopCart, new CookieOptions() { Expires = DateTimeOffset.Now.AddDays(10) });
        }

    }
}