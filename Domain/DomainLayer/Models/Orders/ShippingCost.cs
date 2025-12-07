using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Orders
{
    public class ShippingCost : BaseEntity
    {
        public ShippingCostType Type { get; set; }
        public int Cost { get; set; }
    }
    public enum ShippingCostType
    {
        NimKilo,
        YekKilo,
        YekKiloVa500Gram,
        YekKiloVa500Gram_Ta_DokiloVa500Gram,
        DoKiloVa500Gram,
        SeKilo
    }
}
