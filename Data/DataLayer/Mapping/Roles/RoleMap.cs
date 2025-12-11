using DomainLayer.Models.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Roles
{
    public class RoleMap:IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(c => c.Id);
            builder.ToTable("Roles", "dbo");

            builder.Property(r => r.RoleTitle)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("GetDate()");
        }
    }
}