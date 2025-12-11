using DomainLayer.Models.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Tickets
{
    class TicketMap : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets", "dbo");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CreationDate)
                .HasDefaultValueSql("GetDate()");

            builder.Property(c=>c.TicketBody)
                .IsRequired();

            builder.Property(c => c.TicketTitle)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c=>c.Status)
                .IsRequired()
                .HasDefaultValue(TicketStatus.Waiting_For_Reply);

            builder.HasOne(c => c.User)
                .WithMany(c => c.Tickets)
                .HasForeignKey(c => c.BuilderId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
