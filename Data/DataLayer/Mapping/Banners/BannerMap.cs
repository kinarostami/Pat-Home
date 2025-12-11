using DomainLayer.Models.Banners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Banners
{
    public class BannerMap: IEntityTypeConfiguration<Banner>
    {
        public void Configure(EntityTypeBuilder<Banner> builder)
        {
            builder.ToTable("Banners", "dbo");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Url)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(c => c.ImageName)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(c => c.CreationDate)
                .HasDefaultValueSql("GetDate()");
        }
    }
}