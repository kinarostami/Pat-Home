using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.DTOs.Products;

public class ProductSelectItem
{
    public long ProductId { get; set; }
    public string ProductTitle { get; set; }
    public int Price { get; set; }
    public int Count { get; set; }
}
