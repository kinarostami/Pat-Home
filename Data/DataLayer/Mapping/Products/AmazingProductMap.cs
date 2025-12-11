using DomainLayer.Models.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Products
{
    public class AmazingProductMap: IEntityTypeConfiguration<AmazingProduct>
    {
       
        public void Configure(EntityTypeBuilder<AmazingProduct> builder)
        {
            builder.ToTable("AmazingProducts", "dbo");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("GetDate()");

        }
    }
}