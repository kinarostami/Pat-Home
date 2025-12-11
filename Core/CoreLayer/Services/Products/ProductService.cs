using Common.Application.DateUtil;
using Common.Application.FileUtil;
using Common.Application.SecurityUtil;
using CoreLayer.DTOs.Admin.Products;
using CoreLayer.DTOs.Shop;
using CoreLayer.Mappers;
using DataLayer.Context;
using DomainLayer.Models.Banners;
using DomainLayer.Models.Products;
using DomainLayer.Models.Sliders;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static CoreLayer.Services.Products.IProductService;

namespace CoreLayer.Services.Products;

public class ProductService : BaseService, IProductService
{
    private readonly IInventoryService _inventoryService;
    public ProductService(AppDbContext context, IInventoryService inventoryService) : base(context)
    {
        _inventoryService = inventoryService;
    }

    public IQueryable<Product> GetProducts()
    {
        return Table<Product>();
    }


    public ProductsCategoryFilter GetProductsForCategory(int pageId, int take, string search
        , string[] category, string[] colors, string[] sellers, string[] brands, bool isAvailable)
    {
        var result = Table<Product>()
            .Include(c => c.Details)
            .ThenInclude(c => c.Order)
            .Include(c => c.MainGroup)
            .Include(c => c.ParentGroup)
            .Include(c => c.SubParentGroup)
            .Where(p => p.Status == ProductStatus.Active);


        if (!string.IsNullOrEmpty(search))
        {
            result = result.Where(r => r.ProductTitle.Contains(search) || r.Tags.Contains(search));
        }
        if (isAvailable)
        {
            var availableProducts = _inventoryService.GetAvailableProducts();
            result = result.Where(r => availableProducts.Contains(r.Id));
        }


        if (category.Any())
        {
            foreach (var currentGroup in category)
            {
                var courseIds = new List<long>();

                var ids = result.Where(r =>
                    r.MainGroup.GroupTitle == currentGroup ||
                    r.ParentGroup.GroupTitle == currentGroup ||
                    r.SubParentGroup.GroupTitle == currentGroup).Select(s => s.Id);

                if (ids.Any())
                {
                    courseIds.AddRange(ids);
                }
                result = result.Where(r => courseIds.Contains(r.Id));
            }
        }


        var skip = (pageId - 1) * take;
        var lastedModel = result.Skip(skip).Take(take)
            .Select(p => ProductMapper.MapToCardDto(p, _inventoryService));

        var model = new ProductsCategoryFilter()
        {
            Products = lastedModel.ToList(),
            Categories = category,
            IsAvailable = isAvailable,
            Search = search,
            AvailableGroups = Table<ProductGroup>().Include(c => c.SubGroups).ToList()
        };
        model.GeneratePaging(result, take, pageId);
        return model;
    }

    public async Task<MainPageDto> GetMainPageValues()
    {
        var products = Table<Product>()
            .Include(c => c.Details)
            .ThenInclude(c => c.Order)
            .Where(p => p.Status == ProductStatus.Active);

        #region TopVisit
        var topVisit = products.OrderByDescending(d => d.ProductVisit).Take(8)
            .Select(p => ProductMapper.MapToCardDto(p, _inventoryService));
        #endregion

        #region Popular

        var latest =
            ConvertProductToProductCard(products, 8);

        #endregion

        #region Amazings
        var productsL = Table<AmazingProduct>()
            .Include(c => c.Product)
            .Where(p => p.EndDate.Date == DateTime.Now.Date && DateTime.Now.Hour <= 24)
            .Select(s => s.Product);

        var amazings = ConvertProductToProductCard(productsL, 8);
        #endregion


        return new MainPageDto()
        {
            Banners = Table<Banner>().Where(b => b.IsActive).ToList(),
            Sliders = Table<ShopSlider>().Where(s => s.IsActive).ToList(),
            TopVisit = await topVisit.ToListAsync(),
            PopularProducts = await latest.ToListAsync(),
            AmazingProducts = await amazings.ToListAsync(),
            ProductGroups = Table<ProductGroup>().Where(p => p.ParentId == null)
                .OrderByDescending(d => d.Id).Take(6).ToList(),
        };
    }

    public async Task<AdminProductsViewModel> GetProductsForAdmin(int pageId, int take, string title, string shortLink, ProductStatus status, string groupTitle)
    {
        var result = Table<Product>()
            .Include(p => p.MainGroup)
            .Include(p => p.SubParentGroup)
            .Include(p => p.ParentGroup)
            .Include(c => c.Details)
            .ThenInclude(c => c.Order)
            .AsQueryable();
        if (!string.IsNullOrEmpty(title))
        {
            result = result.Where(r => r.ProductTitle.Contains(title));
        }
        if (!string.IsNullOrEmpty(shortLink))
        {
            result = result.Where(r => r.ShortLink == shortLink);
        }
        if (!string.IsNullOrEmpty(groupTitle))
        {
            result = result.Where(r => r.MainGroup.GroupTitle.Contains(groupTitle) || r.ParentGroup.GroupTitle.Contains(groupTitle) || r.SubParentGroup.GroupTitle.Contains(groupTitle));
        }
        if (status != 0)
        {
            result = result.Where(r => r.Status == status);
        }


        var skip = (pageId - 1) * take;
        var pageCount = (int)Math.Ceiling(result.Count() / (double)take);

        return new AdminProductsViewModel()
        {
            Products = await result.Skip(skip).Take(take).ToListAsync(),
            StartPage = (pageId - 4 <= 0) ? 1 : pageId - 4,
            EndPage = (pageId + 5 > pageCount) ? pageCount : pageId + 5,
            CurrentPage = pageId,
            PageCount = pageCount,
            EntityCount = result.Count(),
            GroupTitle = groupTitle,
            ProductStatus = status,
            ShortLink = shortLink,
            ProductName = title,
            Take = take
        };
    }

    public async Task<bool> AddNewProduct(AddProductViewModel productModel)
    {
        if (productModel.CoverImage == null) return false;
        if (!productModel.CoverImage.IsImage()) return false;

        var imageName = await SaveFileInServer.SaveFile(productModel.CoverImage, Directories.ProductImage);
        await SaveFileInServer.SaveFile(productModel.CoverImageForBitMap, Directories.BitMapProductImage, imageName);
        var product = ConvertProductModelToProduct(productModel.Product);
        try
        {
            product.ImageName = imageName;
            product.Specifications = AddProductSpecification(productModel.Values, productModel.Keys);
            if (productModel.Images != null)
            {
                product.Galleries = await AddProductGallery(productModel.Images);
            }
            Insert(product);
            await Save();
            return true;
        }
        catch
        {
            //Delete Images
            DeleteFileFromServer.DeleteFile(imageName, Directories.ProductImage);
            DeleteFileFromServer.DeleteFile(imageName, Directories.BitMapProductImage);
            if (product.Galleries != null)
            {
                foreach (var item in product.Galleries)
                {
                    DeleteFileFromServer.DeleteFile(item.ImageName, Directories.ProductGallery);
                }
            }
            return false;
        }
    }

    public async Task<bool> EditProduct(AddProductViewModel productModel)
    {
        //Convert Model To Product
        var product = await ConvertProductModelToOriginalProduct(productModel.Product);
        var imageName = product.ImageName;

        //-->نام عکس های قدیمی رو ذخیره میکنیم تا بعد از ویرایش حذف شوند
        var oldCoverImage = product.ImageName;
        var galleriesImage = "";
        foreach (var gallery in product.Galleries)
        {
            galleriesImage += $"{gallery.ImageName},";
        }
        if (!string.IsNullOrEmpty(galleriesImage))
        {
            galleriesImage = galleriesImage.Substring(0, galleriesImage.Length - 1);
        }
        //<--End Save Old Values

        if (productModel.CoverImage != null)
        {
            if (!productModel.CoverImage.IsImage()) return false;
            imageName = await SaveFileInServer.SaveFile(productModel.CoverImage, Directories.ProductImage);
            product.ImageName = imageName;
        }
        if (productModel.CoverImageForBitMap != null)
        {
            if (productModel.CoverImage == null)
                DeleteFileFromServer.DeleteFile(imageName, Directories.BitMapProductImage);

            await SaveFileInServer.SaveFile(productModel.CoverImageForBitMap, Directories.BitMapProductImage, imageName);
        }
        //
        try
        {
            //Set Relations Values
            product.Specifications = EditProductSpecification(productModel.Values, productModel.Keys, product.Id);
            if (productModel.Images != null)
            {
                product.Galleries = await UpdateGallery(productModel.Images, product.Id);
            }
            Update(product);
            await Save();
            //وقتی که محصول با موفقیت ویرایش شد اگر عکس جدیدی انتخاب کرده بود عکس قدیمی را حذف می کنیم


            if (productModel.CoverImage != null)
                DeleteFileFromServer.DeleteFile(oldCoverImage, Directories.ProductImage);

            if (productModel.CoverImageForBitMap != null)
                if (productModel.CoverImage != null)
                    DeleteFileFromServer.DeleteFile(oldCoverImage, Directories.BitMapProductImage);

            return true;
        }
        catch
        {
            if (productModel.CoverImage != null)
            {
                DeleteFileFromServer.DeleteFile(imageName, Directories.ProductImage);
                DeleteFileFromServer.DeleteFile(imageName, Directories.BitMapProductImage);
            }
            if (productModel.Images == null) return false;
            foreach (var item in product.Galleries!)
            {
                DeleteFileFromServer.DeleteFile(item.ImageName, Directories.ProductGallery);
            }
            return false;
        }
    }

    public async Task<Product> GetProductById(long productId)
    {
        return await _context.Products.AsNoTracking()
            .Include(c => c.Galleries)
            .Include(c => c.Specifications)
            .AsSplitQuery()
            .SingleOrDefaultAsync(p => p.Id == productId);
    }

    public async Task<Product> GetProductWidthRelations(long productId)
    {
        return await Table<Product>()
            .SingleOrDefaultAsync(p => p.Id == productId);
    }

    public async Task<Product> GetProductById(long productId, bool forSinglePage)
    {
        var product = await _context.Products
            .Include(c => c.Galleries)
            .Include(c => c.Specifications)
            .Include(c => c.MainGroup)
            .Include(c => c.ParentGroup)
            .Include(c => c.SubParentGroup)
            .Include(c => c.Comments)
            .ThenInclude(c => c.User)
            .SingleOrDefaultAsync(p => p.Id == productId);

        if (product != null)
        {
            product.ProductVisit += 1;
            Update(product);
            await Save();
        }

        return product;
    }

    public async Task<Product> GetProductByShortLink(string shortLink)
    {
        return await Table<Product>().SingleOrDefaultAsync(p => p.ShortLink == shortLink);
    }

    public async Task<Product> GetProductByIdWithRelations(long productId, string[] relations)
    {
        return await GetById<Product>(productId, relations);
    }
    public async Task<DeleteTypes> DeleteProduct(Product product)
    {
        if (product == null) return DeleteTypes.NotFound;

        Delete(product);
        await Save();
        DeleteFileFromServer.DeleteFile(product.ImageName, Directories.ProductImage);

        foreach (var gallery in product.Galleries)
        {
            DeleteFileFromServer.DeleteFile(gallery.ImageName, Directories.ProductGallery);
        }
        return DeleteTypes.FullDelete;
    }


    public async Task AddComment(ProductComment comment)
    {
        Insert(comment);
        await Save();
    }

    public async Task DeleteComment(long commentId)
    {
        var comment = await GetById<ProductComment>(commentId);
        Delete(comment);
        await Save();
    }

    public async Task DeleteComment(long commentId, long userId)
    {
        var comment = await GetById<ProductComment>(commentId);
        if (comment.UserId != userId)
            throw new Exception();
        Delete(comment);
        await Save();
    }

    public async Task<ProductCommentsViewModel> GetCommentByFilter(int pageId, int take, string startDate, string endDate)
    {
        var result = Table<ProductComment>()
            .Include(c => c.User)
            .Include(c => c.Product).AsQueryable();

        var stDate = startDate.ToMiladi();
        var eDate = endDate.ToMiladi();
        if (!string.IsNullOrEmpty(startDate))
        {
            result = result.Where(r => r.CreationDate.Date >= stDate.Date);
        }
        if (!string.IsNullOrEmpty(endDate))
        {
            result = result.Where(r => r.CreationDate.Date <= eDate.Date);
        }
        var skip = (pageId - 1) * take;
        var pageCount = (int)Math.Ceiling(result.Count() / (double)take);

        return new ProductCommentsViewModel()
        {
            Comments = await result.OrderByDescending(d => d.CreationDate).Skip(skip).Take(take).ToListAsync(),
            StartPage = (pageId - 4 <= 0) ? 1 : pageId - 4,
            EndPage = (pageId + 5 > pageCount) ? pageCount : pageId + 5,
            CurrentPage = pageId,
            PageCount = pageCount,
            EntityCount = result.Count(),
            EndDate = (string.IsNullOrEmpty(endDate) ? null : (DateTime?)eDate),
            StartDate = (string.IsNullOrEmpty(startDate) ? null : (DateTime?)stDate)
        };
    }

    #region Utilities
    private async Task<Product> ConvertProductModelToOriginalProduct(ProductModel productModel)
    {
        var product = await GetById<Product>(productModel.ProductId, "Galleries");
        product.ProductTitle = productModel.ProductTitle.SanitizeText();
        product.GroupId = productModel.GroupId;
        product.ParentGroupId = productModel.ParentGroupId;
        product.ProductDescription = productModel.ProductDescription.SanitizeText();
        product.SubParnetGroupId = (productModel.SubParentGroupId == 0 ? null : productModel.SubParentGroupId);
        product.Status = productModel.Status;
        product.MetaDescription = productModel.MetaDescription.SanitizeText();
        product.Tags = productModel.Tags.SanitizeText();
        product.Gram = productModel.Gram;
        product.DiscountPercentage = productModel.DiscountPercentage;
        return product;
    }
    private Product ConvertProductModelToProduct(ProductModel product)
    {
        return new Product()
        {
            ProductTitle = product.ProductTitle.SanitizeText(),
            GroupId = product.GroupId,
            ParentGroupId = product.ParentGroupId,
            UserId = product.UserId,
            SubParnetGroupId = product.SubParentGroupId,
            Status = product.Status,
            MetaDescription = product.MetaDescription.SanitizeText(),
            ProductDescription = product.ProductDescription.SanitizeText(),
            ProductVisit = 0,
            ShortLink = GenerateShortKey(4),
            Tags = product.Tags.SanitizeText(),
            Gram = product.Gram,
            DiscountPercentage = product.DiscountPercentage
        };
    }

    private async Task<List<ProductGallery>> UpdateGallery(List<IFormFile> inputs, long productId)
    {
        //Delete Old Values
        var oldGalleries = TableTracking<ProductGallery>().Where(g => g.ProductId == productId);
        if (oldGalleries.Any())
        {
            Delete(oldGalleries);
        }
        //End Delete

        var galleries = new List<ProductGallery>();
        foreach (var image in inputs)
        {
            if (!image.IsImage()) continue;
            var fileName = await SaveFileInServer.SaveFile(image, Directories.ProductGallery);
            galleries.Add(new ProductGallery()
            {
                ImageName = fileName,
            });
        }

        return galleries;
    }
    private async Task<List<ProductGallery>> AddProductGallery(List<IFormFile> inputs)
    {
        var galleries = new List<ProductGallery>();
        foreach (var image in inputs)
        {
            if (!image.IsImage()) continue;
            var fileName = await SaveFileInServer.SaveFile(image, Directories.ProductGallery);
            galleries.Add(new ProductGallery()
            {
                ImageName = fileName,
            });
        }

        return galleries;
    }


    private List<ProductSpecifications> AddProductSpecification(List<string> values, List<string> keys)
    {
        var valueLength = values.Count;
        var keyLength = keys.Count;
        var specifications = new List<ProductSpecifications>();
        for (var i = 0; i < keyLength; i++)
        {
            if (i > valueLength) break;
            if (string.IsNullOrEmpty(keys[i])) continue;

            specifications.Add(new ProductSpecifications()
            {
                Key = keys[i],
                Value = values[i],
            });
        }

        return specifications;
    }
    private List<ProductSpecifications> EditProductSpecification(List<string> values, List<string> keys, long productId)
    {
        //Delete Old Values
        var oldValues = TableTracking<ProductSpecifications>()
            .Where(s => s.ProductId == productId);
        if (oldValues.Any())
        {
            Delete(oldValues);
        }
        //End Delete
        var valueLength = values.Count;
        var keyLength = keys.Count;
        var specifications = new List<ProductSpecifications>();
        for (var i = 0; i < keyLength; i++)
        {
            if (i > valueLength) break;
            if (string.IsNullOrEmpty(keys[i])) continue;

            specifications.Add(new ProductSpecifications()
            {
                Key = keys[i],
                Value = values[i],
            });
        }

        return specifications;
    }

    private string GenerateShortKey(int length)
    {
        //در این جا یک کلید با طول دلخواه تولید میکنیم
        var key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, length);

        while (_context.Products.Any(p => p.ShortLink == key))
        {
            //تا زمانی که کلید ساخته شده تکراری باشد این عملیات تکرار میشود

            key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, length);
        }
        //در آخر یک کلید غیره تکراری با طول دلخواه ساخته شده
        return key;
    }

    private IQueryable<ProductCardDto> ConvertProductToProductCard(IQueryable<Product> products, int take)
    {
        return products.OrderByDescending(d => d.Id).Take(take)
            .Select(p => ProductMapper.MapToCardDto(p, _inventoryService));
    }
    #endregion
}

