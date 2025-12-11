using CoreLayer.DTOs.Admin.Products;
using CoreLayer.DTOs.Shop;
using DomainLayer.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Products;

public interface IProductService
{
    IQueryable<Product> GetProducts();
    ProductsCategoryFilter GetProductsForCategory(int pageId, int take, string search
        , string[] category, string[] colors, string[] sellers, string[] brands, bool isAvalibale);
    Task<MainPageDto> GetMainPageValues();
    Task<AdminProductsViewModel> GetProductsForAdmin(int pageId, int take, string title, string shortLink,
            ProductStatus status, string groupTitle);
    Task<bool> AddNewProduct(AddProductViewModel productModel);
    Task<bool> EditProduct(AddProductViewModel productModel);
    Task<Product> GetProductById(long productId);
    Task<Product> GetProductWidthRelations(long productId);
    Task<Product> GetProductById(long productId, bool forSinglePage);
    Task<Product> GetProductByShortLink(string shortLink);
    Task<Product> GetProductByIdWithRelations(long productId, string[] relations);

    /// <summary>
    /// به این متد پاس بدهید  Galleries-OrderDetails  محصول را با رابطه 
    /// </summary>
    /// <param name="product">محصول</param>
    /// <returns>DeleteTypes</returns>
    Task<DeleteTypes> DeleteProduct(Product product);


    Task AddComment(ProductComment comment);
    Task DeleteComment(long commentId);
    Task DeleteComment(long commentId, long userId);
    Task<ProductCommentsViewModel> GetCommentByFilter(int pageId, int take, string startDate, string endDate);

    public enum DeleteTypes
    {
        FullDelete,
        NotFound,
        SoftDelete
    }
}

