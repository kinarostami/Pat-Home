namespace CoreLayer.DTOs.Shop
{
    public class ProductCategoryDto:ProductCardDto
    {
        public bool IsAvailable { get; set; }
        public string SellerName { get; set; }
        public int  Count { get; set; }
    }
}