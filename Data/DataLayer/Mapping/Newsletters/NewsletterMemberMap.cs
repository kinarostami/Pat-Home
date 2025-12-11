using DomainLayer.Models.Newsletters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Newsletters
{
    public class NewsletterMemberMap: IEntityTypeConfiguration<NewsletterMember>
    {
        public void Configure(EntityTypeBuilder<NewsletterMember> builder)
        {
            builder.ToTable("NewsletterMembers", "dbo");
            builder.HasKey(b => b.Id);
            builder.HasIndex(b => b.Email).IsUnique();

            builder.Property(b => b.Email)
                .IsUnicode(false)
                .HasMaxLength(200);

            builder.Property(b => b.MemberCode)
                .IsRequired()
                .HasDefaultValueSql("newid()");

            builder.Property(c => c.CreationDate)
                .HasDefaultValueSql("GetDate()");
        }
    }
}