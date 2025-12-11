using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Products;

public class ProductGroup : BaseEntity
{
    public string GroupTitle { get; set; }
    public string GroupImage { get; set; }
    public long? ParentId { get; set; }

    public ICollection<ProductGroup> SubGroups { get; set; }
    public ICollection<Product> ProductsMainGroup { get; set; }
    public ICollection<Product> ProductsParentGroup { get; set; }
    public ICollection<Product> ProductsSubParentGroup { get; set; }
}
