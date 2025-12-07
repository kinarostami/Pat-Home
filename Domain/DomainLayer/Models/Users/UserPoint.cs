using DomainLayer.Models.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Users;

public class UserPoint : BaseEntity
{
    public long UserId { get; set; }
    public WalletType Type { get; set; }
    public int Count { get; set; }
    public string Description { get; set; }

    public User User { get; set; }
}
