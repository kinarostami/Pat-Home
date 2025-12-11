using System;
using System.Collections.Generic;
using CoreLayer.DTOs.Pagination;

namespace CoreLayer.DTOs.Shop
{
    public class AmazingProductsViewModel:BasePaging
    {
        public List<ProductCategoryDto> AmazingProducts { get; set; }
        public string OrderBy { get; set; }
        public DateTime EndDate { get; set; }
    }
}