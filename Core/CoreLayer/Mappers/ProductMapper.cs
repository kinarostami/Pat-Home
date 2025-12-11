using CoreLayer.DTOs.Shop;
using DataLayer.Migrations;
using DomainLayer.Models.Products;
using InventoryManagement.Application.ApplicationServices.Inventories;

namespace CoreLayer.Mappers
{
    public class ProductMapper
    {
        public static ProductCardDto MapToCardDto(Product product, IInventoryService service)
        {
            var inventory = service.GetFirstInventoryByProductId(product.Id).Result;
            int? newPrice = null;
            if (product.DiscountPercentage > 0)
            {
                var discount = product.DiscountPercentage * inventory.Price / 100;
                newPrice = inventory.Price - discount;
            }
            return new ProductCardDto()
            {
                DiscountPercentage = product.DiscountPercentage == 0 ? null : product.DiscountPercentage,
                NewPrice = newPrice,
                Price = inventory.Price,
                ProductId = product.Id,
                ProductImage = product.ImageName,
                Title = product.ProductTitle,
                IsAvailable = service.IsAvailable(product.Id)
            };
        }
    }
}