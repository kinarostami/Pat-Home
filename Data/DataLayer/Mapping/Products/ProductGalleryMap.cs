using DomainLayer.Models.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Products
{
    class ProductGalleryMap : IEntityTypeConfiguration<ProductGallery>
    {
        public void Configure(EntityTypeBuilder<ProductGallery> builder)
        {
            builder.ToTable("ProductGalleries", "dbo");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.ImageName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("GetDate()");

            builder.HasOne(c => c.Product)
                .WithMany(c => c.Galleries)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
