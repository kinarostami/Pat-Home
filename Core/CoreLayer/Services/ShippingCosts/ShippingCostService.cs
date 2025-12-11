using DataLayer.Context;
using DomainLayer.Models.Orders;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreLayer.Services.ShippingCosts
{
    public class ShippingCostService : BaseService, IShippingCostService
    {
        public ShippingCostService(AppDbContext context) : base(context)
        {
        }

        public async Task Add(ShippingCost shippingCost)
        {
            var existInDataBase=await Table<ShippingCost>().AnyAsync(f=>f.Type==shippingCost.Type);
            if (existInDataBase)
                return;
            Insert(shippingCost);
            await Save();
        }

        public async Task Edit(ShippingCost shippingCost)
        {
            Update(shippingCost);
            await Save();
        }

        public Task<ShippingCost> GetById(long id)
        {
           return GetById<ShippingCost>(id);
        }

        public async Task<List<ShippingCost>> GetShippingCosts()
        {
            return await Table<ShippingCost>().ToListAsync();
        }
    }
}
