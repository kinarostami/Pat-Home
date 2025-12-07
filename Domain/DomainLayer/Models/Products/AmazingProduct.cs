using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Products;

public class AmazingProduct : BaseEntity
{
    public long ProductId { get; set; }
    public DateTime EndDate { get; set; }

    [ForeignKey("ProductId")]
    public Product Product { get; set; }
}
