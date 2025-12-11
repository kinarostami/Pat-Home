using DomainLayer.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.ShippingCosts
{
    public interface IShippingCostService
    {
        public Task<List<ShippingCost>> GetShippingCosts();
        public Task<ShippingCost> GetById(long id);
        public Task Add(ShippingCost shippingCost);
        public Task Edit(ShippingCost shippingCost);
    }
}
