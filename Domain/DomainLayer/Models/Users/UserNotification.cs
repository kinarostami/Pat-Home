using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Users;

public class UserNotification : BaseEntity
{
    public long UserId { get; set; }
    public string NotificationTitle { get; set; }
    public string NotificationBody { get; set; }
    public bool IsSeen { get; set; }
    public User User { get; set; }
}
