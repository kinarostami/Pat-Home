using DomainLayer.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Users
{
    class UserAddressMap : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.ToTable("UserAddresses", "dbo");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Address)
                .IsRequired()
                .HasMaxLength(5000);

            builder.Property(c => c.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Family)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c=>c.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.NationalCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(c => c.Phone)
                .IsRequired().HasMaxLength(11);

            builder.Property(c => c.PostalCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(c => c.Shire)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("GetDate()");

            builder.HasOne(c => c.User)
                .WithMany(c => c.Addresses)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
