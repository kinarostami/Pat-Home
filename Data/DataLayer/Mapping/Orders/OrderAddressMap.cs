using DomainLayer.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Orders
{
    public class OrderAddressMap : IEntityTypeConfiguration<OrderAddress>
    {

        public void Configure(EntityTypeBuilder<OrderAddress> builder)
        {
            builder.ToTable("OrderAddresses", "dbo");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Address)
                .IsRequired();

            builder.Property(c => c.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Family)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.NationalCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(c => c.Phone)
                .IsRequired()
                .HasMaxLength(11);

            builder.Property(c => c.PostalCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(c => c.Shire)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("GetDate()");
        }
    }
}