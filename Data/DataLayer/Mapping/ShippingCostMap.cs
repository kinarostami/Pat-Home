using DomainLayer.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Mapping
{
    internal class ShippingCostMap : IEntityTypeConfiguration<ShippingCost>
    {
        public void Configure(EntityTypeBuilder<ShippingCost> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("ShippingCosts", "dbo");


            builder.HasData(new List<ShippingCost>()
            {
                new ShippingCost()
                {
                    Id = 1,
                    Cost = 15000,
                    Type=ShippingCostType.NimKilo
                },
                new ShippingCost()
                {
                    Id = 2,
                    Cost = 20000,
                    Type=ShippingCostType.YekKilo
                },
                new ShippingCost()
                {
                    Id = 3,
                    Cost = 22000,
                    Type=ShippingCostType.YekKiloVa500Gram
                },
                new ShippingCost()
                {
                    Id = 4,
                    Cost = 24000,
                    Type=ShippingCostType.YekKiloVa500Gram_Ta_DokiloVa500Gram
                },
                new ShippingCost()
                {
                    Id = 5,
                    Cost = 25000,
                    Type=ShippingCostType.DoKiloVa500Gram
                },
                new ShippingCost()
                {
                    Id = 6,
                    Cost = 20000,
                    Type=ShippingCostType.SeKilo
                }
            });
        }
    }
}
