using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Users;

public class UserCard : BaseEntity
{
    public long UserId { get; set; }
    public string CardNumber { get; set; }
    public string ShabahNumber { get; set; }
    public bool IsAccept { get; set; }
    public string BankName { get; set; }
    public string OwnerName { get; set; }
    public string AccountNumber { get; set; }


    public User User { get; set; }
}