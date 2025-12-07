using System;
using DomainLayer.Models.Users;

namespace DomainLayer.Models.Wallets;

public class Wallet : BaseEntity
{
    public long UserId { get; set; }
    public int Amount { get; set; }
    public string Description { get; set; }
    public string PayWith { get; set; }
    public string Authority { get; set; }
    public long? RefId { get; set; }
    public bool IsFinally { get; set; }
    public WalletType WalletType { get; set; }

    public DateTime? PaymentDate { get; set; }


    public User User { get; set; }

}
public enum WalletType
{
    واریز = 1,
    برداشت = 2
}
