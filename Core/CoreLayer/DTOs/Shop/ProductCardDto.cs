namespace CoreLayer.DTOs.Shop
{
    public class ProductCardDto
    {
        public long ProductId { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public int? NewPrice { get; set; }
        public string ProductImage { get; set; }
        public int? DiscountPercentage { get; set; }
        public bool IsAvailable { get; set; }
    }
}