using InventoryManagement.Domain.Models.InventoryAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Infrastructure.Persistent.EF.Mapping;

public class InventoryMap : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.ToTable("Inventories", "dbo");

        builder.HasKey(b => b.Id);
        builder.HasIndex(b => b.ProductId);
    }
}
