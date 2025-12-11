using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping
{
    class SiteRulesMap : IEntityTypeConfiguration<SiteRules>
    {
        public void Configure(EntityTypeBuilder<SiteRules> builder)
        {
            builder.HasKey(b => b.Id);
            builder.ToTable("SiteRules", "dbo");
            builder.Property(b => b.MainRule)
                .HasDefaultValue("در حال حاضر قوانینی وجود ندارد");

            builder.Property(c => c.CreationDate)
                .HasDefaultValueSql("GetDate()");
        }
    }
}
