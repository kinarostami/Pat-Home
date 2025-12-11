using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping
{
    public class PopUpModelMap:IEntityTypeConfiguration<PopUpModel>
    {
        public void Configure(EntityTypeBuilder<PopUpModel> builder)
        {
            builder.ToTable("PopUpModel", "dbo");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.LinkText)
                .HasMaxLength(100);

            builder.Property(b => b.LinkUrl)
                .HasMaxLength(300);

            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("GEtDate()");
        }
    }
}