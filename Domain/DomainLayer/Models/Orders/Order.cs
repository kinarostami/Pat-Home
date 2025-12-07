using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Orders;

public class Order : BaseEntity
{
    public long UserId { get; set; }
}
