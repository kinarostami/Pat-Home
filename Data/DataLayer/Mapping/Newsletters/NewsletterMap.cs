
using DomainLayer.Models.Newsletters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Newsletters
{
    public class NewsletterMap : IEntityTypeConfiguration<Newsletter>
    {
        public void Configure(EntityTypeBuilder<Newsletter> builder)
        {
            builder.ToTable("Newsletters", "dbo");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("GetDate()");

            builder.Property(b => b.Subject)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(b => b.IsSend)
                .HasDefaultValue(0);

            builder.Property(b => b.Body)
                .IsRequired();
        }
    }
}