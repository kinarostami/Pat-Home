using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Products;

public  class ProductGallery : BaseEntity
{
    public long ProductId { get; set; }
    public string ImageName { get; set; }

    public Product Product { get; set; }
}
