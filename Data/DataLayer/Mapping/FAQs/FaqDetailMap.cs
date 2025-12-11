using DomainLayer.Models.FAQs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.FAQs
{
    public class FaqDetailMap: IEntityTypeConfiguration<FaqDetail>
    {
        public void Configure(EntityTypeBuilder<FaqDetail> builder)
        {
            builder.ToTable("FaqDetails", "dbo");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(b=>b.Description)
                .IsRequired();

            builder.Property(c => c.CreationDate)
                .HasDefaultValueSql("GetDate()");

            builder.Property(b => b.Icon)
                .IsUnicode(false)
                .HasMaxLength(100);

            builder.HasOne(b => b.Faq)
                .WithMany(b => b.Children)
                .HasForeignKey(b => b.FaqId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}