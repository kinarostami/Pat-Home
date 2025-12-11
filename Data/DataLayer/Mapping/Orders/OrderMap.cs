using DomainLayer.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Orders
{
    class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders", "dbo");
            builder.Ignore(b => b.TotalPrice);
            builder.HasKey(c => c.Id);


            builder.Property(c => c.Price)
                           .IsRequired();

            builder.Property(c => c.ShippingCost)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(c => c.Discount)
                .IsRequired(false);

            builder.Property(c => c.DiscountTitle)
                .IsRequired(false);

            builder.Property(c => c.SendTypeTitle)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(c => c.ItemCount)
                .HasDefaultValue(1);

            builder.Property(c => c.IsFinally)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(c => c.IsSendFactor)
               .IsRequired()
               .HasDefaultValue(false);

            builder.Property(c => c.ByWallet)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(c => c.Status)
               .IsRequired()
               .HasDefaultValue(OrderStatus.در_انتظار_پرداخت);

            builder.Property(c => c.CreationDate)
              .HasDefaultValueSql("GetDate()");

            builder.Property(c => c.PaymentDate)
                .HasDefaultValueSql("GetDate()")
                .IsRequired();


            builder.HasOne(c => c.User)
                .WithMany(c => c.Orders)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Address)
                .WithOne(c => c.Order)
                .HasForeignKey<OrderAddress>(c => c.OrderId);

        }
    }
}
