using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer;

public class BaseEntity
{
    public long Id { get; set; }
    public DateTime CreationDate { get; set; }
    public BaseEntity()
    {
        CreationDate = DateTime.Now;
    }
}
