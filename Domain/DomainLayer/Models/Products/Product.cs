using DomainLayer.Models.Orders;
using DomainLayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Products;

public class Product : BaseEntity
{
    public long UserId { get; set; }
    public long GroupId { get; set; }
    public long ParentGroupId { get; set; }
    public long? SubParnetGroupId { get; set; }
    public long ProductVisit { get; set; }
    public int Gram { get; set; }
    public int DiscountPercentage { get; set; }
    public string ProductTitle { get; set; }
    public string ProductDescription { get; set; }
    public string MetaDescription { get; set; }
    public string Tags { get; set; }
    public string ImageName { get; set; }
    public string ShortLink { get; set; }
    public ProductStatus Status { get; set; }

    #region Relations
    public User User { get; set; }
    public ProductGroup MainGroup { get; set; }
    public ProductGroup ParentGroup { get; set; }
    public ProductGroup SubParentGroup { get; set; }
    public ICollection<ProductGallery> Galleries { get; set; }
    public ICollection<ProductComment> Comments { get; set; }
    public ICollection<ProductSpecifications> Specifications { get; set; }
    public ICollection<OrderDetail> Details { get; set; }

    #endregion
}
public enum ProductStatus
{
    Active = 1,
    DeActive = 2,
    NewRequest = 3
}
