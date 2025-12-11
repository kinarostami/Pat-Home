using DomainLayer.Models.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Products
{
    class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products", "dbo");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.ImageName)
                .IsRequired()
                .HasMaxLength(300);



            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("GetDate()");

            builder.Property(c => c.GroupId)
              .IsRequired();
            builder.Property(c => c.Status)
              .IsRequired();

            builder.Property(c => c.MetaDescription)
              .IsRequired()
              .HasMaxLength(350);


            builder.Property(c => c.Tags)
                .IsRequired()
                .HasDefaultValue(" ")
                .HasMaxLength(4500);

            builder.Property(c => c.ParentGroupId)
                .IsRequired();

            builder.Property(c => c.SubParnetGroupId)
              .IsRequired(false);

            builder.Property(c => c.ProductDescription)
              .IsRequired();

            builder.Property(c => c.ProductTitle)
              .IsRequired()
              .HasMaxLength(250);

            builder.Property(c => c.ProductVisit)
              .HasDefaultValue(0);

            builder.Property(c => c.ShortLink)
                .IsRequired()
                .HasMaxLength(8)
                .IsUnicode(false);

            builder.HasOne(c => c.User);


            builder.HasOne(c => c.MainGroup)
                .WithMany(c => c.ProductsMainGroup)
                .HasForeignKey(c => c.GroupId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(c => c.ParentGroup)
                .WithMany(c => c.ProductsParentGroup)
                .HasForeignKey(c => c.ParentGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.SubParentGroup)
                .WithMany(c => c.ProductsSubParentGroup)
                .HasForeignKey(c => c.SubParnetGroupId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
