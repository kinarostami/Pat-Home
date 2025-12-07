using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.DiscountCodes;

public class DiscountCode : BaseEntity
{
    public string CodeTitle { get; set; }
    public int? Price { get; set; }
    public int? Percentage { get; set; }
    public int Count { get; set; }
    public int UsedCount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
