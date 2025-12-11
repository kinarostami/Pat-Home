using DomainLayer.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Users
{
    class UserCardMap : IEntityTypeConfiguration<UserCard>
    {
        public void Configure(EntityTypeBuilder<UserCard> builder)
        {
            builder.ToTable("UserCards", "dbo");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CardNumber)
                .IsRequired()
                .HasMaxLength(16);

            builder.Property(c => c.BankName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.BankName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.ShabahNumber)
                .IsRequired()
                .HasMaxLength(24);

            builder.Property(c => c.AccountNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("GetDate()");

            builder.HasOne(c => c.User)
                .WithMany(c => c.UserCards)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
