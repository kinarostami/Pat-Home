using DomainLayer.Models.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Roles
{
    public class RolePermissionMap: IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(p => p.Id);
            builder.ToTable("RolePermissions", "dbo");
           
            builder.Property(r => r.PermissionId)
                .IsRequired();

            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("GetDate()");

            builder.HasOne(r => r.Role)
               .WithMany(r => r.RolePermissions)
               .HasForeignKey(r => r.RoleId)
               .OnDelete(DeleteBehavior.Cascade);
            ;
        }
    }
}