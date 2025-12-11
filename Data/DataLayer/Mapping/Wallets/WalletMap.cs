using DomainLayer.Models.Wallets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Wallets
{
    class WalletMap : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("Wallets", "dbo");
            builder.HasKey(w => w.Id);

            builder.Property(w => w.PaymentDate)
                .IsRequired(false)
                .HasDefaultValueSql("GetDate()");


            builder.Property(c => c.CreationDate)
                .HasDefaultValueSql("GetDate()");

            builder.Property(w => w.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(w => w.Amount)
                .IsRequired();

            builder.HasOne(w => w.User)
                .WithMany(w => w.Wallets)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
