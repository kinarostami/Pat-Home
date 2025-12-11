using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping
{
    class ContactUsMap : IEntityTypeConfiguration<ContactUs>
    {
        public void Configure(EntityTypeBuilder<ContactUs> builder)
        {
            builder.ToTable("ContactUs", "dbo");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Answer)
                .IsRequired(false);

            builder.Property(c => c.FullName)
                .IsRequired()
                .HasMaxLength(350);

            builder.Property(c => c.CreationDate)
                .HasDefaultValueSql("getDate()");

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.IsSeenAdmin)
                .HasDefaultValue(false);

            builder.Property(c => c.MessageBody)
                .IsRequired();

            builder.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(11);

            builder.Property(c => c.Subject)
                .IsRequired()
                .HasMaxLength(50);

        }
    }
}
