using DomainLayer.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Users
{
    class UserNotificationMap : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        {
            builder.ToTable("UserNotifications", "dbo");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.IsSeen)
                .HasDefaultValue(false);

            builder.Property(c => c.NotificationBody)
                .IsRequired();

            builder.Property(c => c.NotificationTitle)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(c => c.CreationDate)
               .HasDefaultValueSql("GetDate()");

            builder.HasOne(c => c.User)
                .WithMany(c => c.UserNotifications)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
