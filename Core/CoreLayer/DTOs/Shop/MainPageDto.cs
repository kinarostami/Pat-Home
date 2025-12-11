using System.Collections.Generic;
using DomainLayer.Models.Banners;
using DomainLayer.Models.Products;
using DomainLayer.Models.Sliders;

namespace CoreLayer.DTOs.Shop
{
    public class MainPageDto
    {
        public List<ShopSlider> Sliders { get; set; }
        public List<Banner> Banners { get; set; }
        public List<ProductGroup> ProductGroups { get; set; }
        public List<ProductCardDto> AmazingProducts { get; set; }
        public List<ProductCardDto> PopularProducts { get; set; }
        public List<ProductCardDto> TopVisit { get; set; }
    }
}