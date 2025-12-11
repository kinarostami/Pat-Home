using DataLayer.Context;
using DomainLayer.Models.Orders;
using DomainLayer.Models.Orders.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.ShippingCosts
{
    public class ShippingCostDomainService :BaseService, IShippingCostDomainService
    {
        public ShippingCostDomainService(AppDbContext context) : base(context)
        {
        }

        public List<ShippingCost> GetShippingCosts()
        {
            return Table<ShippingCost>().ToList();
        }
    }
}
