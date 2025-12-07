using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Orders.DomainServices
{
    public interface IShippingCostDomainService
    {
        public List<ShippingCost> GetShippingCosts();
    }
}
