using DomainLayer.Models.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Products
{
    class ProductGroupMap : IEntityTypeConfiguration<ProductGroup>
    {
        public void Configure(EntityTypeBuilder<ProductGroup> builder)
        {
            builder.ToTable("ProductGroups", "dbo");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.ParentId)
                .IsRequired(false);

            builder.Property(c => c.GroupTitle)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.GroupImage)
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("GetDate()");

            builder.HasMany(c => c.SubGroups)
                .WithOne()
                .HasForeignKey(c=>c.ParentId);

        }
    }
}
