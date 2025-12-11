using DomainLayer.Models.DiscountCodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.DiscountCodes
{
    public class DiscountCodeMap: IEntityTypeConfiguration<DiscountCode>
    {
        public void Configure(EntityTypeBuilder<DiscountCode> builder)
        {
            builder.ToTable("DiscountCodes", "dbo");
            builder.HasKey(b => b.Id);
            builder.HasIndex(b => b.CodeTitle).IsUnique();

            builder.Property(b => b.CodeTitle)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(100);

            builder.Property(b => b.StartDate)
                .HasDefaultValueSql("GetDate()");

            builder.Property(b => b.EndDate)
                .HasDefaultValueSql("GetDate()");

            builder.Property(c => c.CreationDate)
                .HasDefaultValueSql("GetDate()");
        }
    }
}