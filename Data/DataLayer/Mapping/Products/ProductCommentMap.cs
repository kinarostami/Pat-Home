using DomainLayer.Models.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Products
{
    class ProductCommentMap : IEntityTypeConfiguration<ProductComment>
    {
        public void Configure(EntityTypeBuilder<ProductComment> builder)
        {
            builder.ToTable("ProductComments", "dbo");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.AnswerId)
                .IsRequired(false);

            builder.Property(c => c.CreationDate)
                .HasDefaultValueSql("getDate()");

            builder.Property(c => c.Text)
                .IsRequired();

            builder.HasOne(c => c.Product)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
