using CoreLayer.DTOs.Pagination;
using DomainLayer.Models.Products;

namespace CoreLayer.DTOs.Admin.Products
{
    public class AdminProductsViewModel:BasePaging
    {
        public List<Product> Products { get; set; }
        public string ProductName { get; set; }
        public string ShortLink { get; set; }
        public string GroupTitle { get; set; }
        public ProductStatus ProductStatus { get; set; }
    }
}