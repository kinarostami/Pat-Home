namespace DomainLayer.Models.Products
{
    public class ProductSpecifications:BaseEntity
    {
        public long ProductId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public Product Product { get; set; }
    }
}