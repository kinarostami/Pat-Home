using DomainLayer.Models.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainLayer.Models.Tickets;

public class TicketMessage : BaseEntity
{
    public long TicketId { get; set; }
    public long UserId { get; set; }
    public string MessageBody { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
    public Ticket Ticket { get; set; }
}