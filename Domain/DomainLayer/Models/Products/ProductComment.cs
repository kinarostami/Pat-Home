using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Products;

public class ProductComment : BaseEntity
{
    public long UserId { get; set; }
    public long ProductId { get; set; }
    public string Text { get; set; }
    public long? AnswerdId { get; set; }

    //[ForeignKey("UserId")]
    //public User User { get; set; }
    public Product Product { get; set; }
    [ForeignKey("AnswerId")]
    public ICollection<ProductComment> CourseComments { get; set; }
}
