using DomainLayer.Models.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Products
{
    class ProductSpecificationsMap : IEntityTypeConfiguration<ProductSpecifications>
    {
        public void Configure(EntityTypeBuilder<ProductSpecifications> builder)
        {
            builder.ToTable("ProductSpecifications", "dbo");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Key)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Value)
                .IsRequired()
                .HasMaxLength(400);

            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("GetDate()");

            builder.HasOne(c => c.Product)
                .WithMany(c => c.Specifications)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
