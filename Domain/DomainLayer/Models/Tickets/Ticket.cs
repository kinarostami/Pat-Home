using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Tickets;

public class Ticket : BaseEntity
{
    public long BuilderId { get; set; }
    public string TicketTitle { get; set; }
    public string TicketBody { get; set; }
    public TicketStatus Status { get; set; }

    //public User User { get; set; }
    public ICollection<TicketMessage> TicketMessages { get; set; }
}
public enum TicketStatus
{
    Close = 1,
    Replied = 2,
    Waiting_For_Reply = 3,
}
