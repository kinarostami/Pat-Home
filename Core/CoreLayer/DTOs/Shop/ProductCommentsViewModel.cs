using System;
using System.Collections.Generic;
using CoreLayer.DTOs.Pagination;
using DomainLayer.Models.Products;

namespace CoreLayer.DTOs.Shop
{
    public class ProductCommentsViewModel:BasePaging
    {
        public List<ProductComment> Comments { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}