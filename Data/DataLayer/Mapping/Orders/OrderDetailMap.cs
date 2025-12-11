using DomainLayer.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Orders
{
    class OrderDetailMap : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetails", "dbo");
            builder.HasKey(c => c.Id);
            builder.Ignore(c => c.TotalPrice);
            builder.Ignore(c => c.KiloGram);

            builder.Property(c => c.Count)
                .IsRequired();

            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("GetDate()");


            builder.HasOne(c => c.Order)
                .WithMany(c => c.Details)
                .HasForeignKey(c => c.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Product)
                .WithMany(b => b.Details)
                .HasForeignKey(b => b.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
