using DomainLayer.Models.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Mapping.Tickets
{
    class TicketMessageMap : IEntityTypeConfiguration<TicketMessage>
    {
        public void Configure(EntityTypeBuilder<TicketMessage> builder)
        {
            builder.ToTable("TicketMessages", "dbo");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.MessageBody)
                .IsRequired();

            builder.Property(b => b.CreationDate)
                .HasDefaultValueSql("GetDate()");

            builder.HasOne(b => b.Ticket)
                .WithMany(b => b.TicketMessages)
                .HasForeignKey(b => b.TicketId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
