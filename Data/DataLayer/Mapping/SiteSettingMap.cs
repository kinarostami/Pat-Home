using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping
{
    public class SiteSettingMap : IEntityTypeConfiguration<SiteSetting>
    {
        public void Configure(EntityTypeBuilder<SiteSetting> entity)
        {
            entity.ToTable("SiteSettings", "dbo");
            entity.HasKey(u => u.Id);


            entity.Property(s => s.ShopTitle)
                .IsRequired(false)
                .HasMaxLength(100);

            entity.Property(s => s.MagTitle)
                .IsRequired(false)
                .HasMaxLength(100);

            entity.Property(s => s.EnglishSitName)
               .IsRequired(false)
               .HasMaxLength(100);

            entity.Property(s => s.PersianSitName)
               .IsRequired(false)
               .HasMaxLength(100);

            entity.Property(s => s.BaseSiteUrl)
               .IsRequired(false)
               .HasMaxLength(150);

            entity.Property(s => s.InsTaGram)
                .IsRequired(false)
                .HasMaxLength(400);

            entity.Property(s => s.Telegram)
                .IsRequired(false)
                .HasMaxLength(400);

            entity.Property(s => s.LinkDin)
                .IsRequired(false)
                .HasMaxLength(400);

            entity.Property(s => s.PhoneNumber)
                .HasMaxLength(11);

            entity.Property(s => s.Address)
                .IsRequired(false)
                .HasMaxLength(6000);

            entity.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(s => s.EmailPassword)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(s => s.EmailSmtpServer)
                .IsRequired()
                .HasMaxLength(400);

            entity.Property(s => s.EmailSmtpPort)
                .IsRequired();

            entity.Property(s => s.ShopDescription)
                .IsRequired(false)
                .HasMaxLength(360);
            entity.Property(s => s.MagDescription)
                .IsRequired(false)
                .HasMaxLength(360);

            entity.Property(s => s.TelePhone)
                .HasMaxLength(11);

        }
    }
}
