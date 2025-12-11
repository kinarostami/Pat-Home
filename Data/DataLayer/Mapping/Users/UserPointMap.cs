using DomainLayer.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Users
{
    public class UserPointMap:IEntityTypeConfiguration<UserPoint>
    {
        public void Configure(EntityTypeBuilder<UserPoint> builder)
        {
            builder.ToTable("UserPoints", "dbo");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Description)
                .IsRequired()
                .HasMaxLength(900);

            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("GetDate()");

            builder.HasOne(b => b.User)
                .WithMany(b => b.Points)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}