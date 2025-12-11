using CoreLayer.DTOs.Shop;
using CoreLayer.Services.Newsletters;
using DataLayer.Context;
using DomainLayer.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.Services.Products;

public class AmazingProductService : BaseService, IAmazingProductService
{
    private INewsletterService _newsletter;
    public AmazingProductService(AppDbContext context, INewsletterService newsletter) : base(context)
    {
        _newsletter = newsletter;
    }

    public async Task<bool> AddProductToAmazing(long productId)
    {
        if (IsProductAmazing(productId)) return false;

        var amazing = new AmazingProduct()
        {
            CreationDate = DateTime.Now,
            ProductId = productId
        };

        var endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
        amazing.EndDate = endDate;
        Insert(amazing);
        try
        {
            await Save();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private bool IsProductAmazing(long productId)
    {
        return Table<AmazingProduct>().Any(a => a.ProductId == productId);
    }

    public async Task<bool> AddProductToAmazing(List<long> productIds)
    {
        var products = new List<Product>();
        foreach (var productId in productIds)
        {
            if (IsProductAmazing(productId)) continue;
            var product = await GetById<Product>(productId);
            if (product == null) continue;
            var amazing = new AmazingProduct()
            {
                CreationDate = DateTime.Now,
                ProductId = productId,
            };

            var endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);
            amazing.EndDate = endDate;
            Insert(amazing);
            products.Add(product);
        }

        try
        {
            await Save();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task DeleteAmazingProducts()
    {
        try
        {
            var entities = TableTracking<AmazingProduct>().Where(e => e.EndDate < DateTime.Now);
            if (entities.Any())
            {
                Delete(entities);
                await Save();
            }
        }
        catch
        {
            //ingored
        }
    }

    public async Task DeleteProductFromAmazing(long amazingId)
    {
        var amazing = await _context.AmazingProducts.FindAsync(amazingId);
        Delete(amazing);
        await Save();
    }

    public Task<AmazingProductsViewModel> GetAmazingProducts(int pageId, int take, string orderBy)
    {
        return null;
    }

    public IQueryable<AmazingProduct> GetAmazingProducts(string search)
    {
        var res = Table<AmazingProduct>()
                .Include(a => a.Product).AsQueryable();
        if (!string.IsNullOrEmpty(search))
        {
            res = res.Where(r => r.Product.ProductTitle.Contains(search));
        }

        return res;
    }

    public async Task<bool> ProductIsAmazing(long productId)
    {
        return await Table<AmazingProduct>().AnyAsync(a => a.EndDate.Date == DateTime.Now.Date && a.ProductId == productId);
    }
}
