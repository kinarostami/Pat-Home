using System.Collections.Generic;
using CoreLayer.DTOs.Pagination;
using DomainLayer.Models.Products;

namespace CoreLayer.DTOs.Shop
{
    public class ProductsCategoryFilter:BasePaging
    {
        public List<ProductCardDto> Products { get; set; }
        public string[] Categories { get; set; }
        public string Search { get; set; }
        public bool IsAvailable { get; set; }
        public List<ProductGroup> AvailableGroups { get; set; }
    }
}
