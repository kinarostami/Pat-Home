using DomainLayer.Models.Orders;

namespace CoreLayer.DTOs.Shop
{
    public class AddProductToCartDto
    {
        public long Id { get; set; }
        public long InventoryId { get; set; }
        public int Count { get; set; } = 1;
        public long UserId { get; set; }
        public StackType StackType { get; set; }
    }
}