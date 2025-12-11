using CoreLayer.DTOs.Shop;
using DomainLayer.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Products;

public interface IAmazingProductService
{
    Task<AmazingProductsViewModel> GetAmazingProducts(int pageId, int take, string orderBy);
    IQueryable<AmazingProduct> GetAmazingProducts(string search);
    Task<bool> AddProductToAmazing(long productId);
    Task<bool> ProductIsAmazing(long productId);
    Task<bool> AddProductToAmazing(List<long> productIds);
    Task DeleteProductFromAmazing(long amazingId);
    Task DeleteAmazingProducts();
}
