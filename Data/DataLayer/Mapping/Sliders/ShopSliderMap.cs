using DomainLayer.Models.Sliders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Sliders
{
    class ShopSliderMap : IEntityTypeConfiguration<ShopSlider>
    {
        public void Configure(EntityTypeBuilder<ShopSlider> builder)
        {
            builder.ToTable("ShopSliders", "dbo");
            builder.HasKey(c=>c.Id);


            builder.Property(c => c.Url)
                .IsRequired();

            builder.Property(c => c.ImageName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Title)
                .HasMaxLength(500);

            builder.Property(c => c.Description)
                .HasMaxLength(1000);

            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("GetDate()");
        }
    }
}
