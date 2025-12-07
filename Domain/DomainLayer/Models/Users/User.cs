using DomainLayer.Models.Articles;
using DomainLayer.Models.Orders;
using DomainLayer.Models.Tickets;
using DomainLayer.Models.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Users;

public class User : BaseEntity
{
    public string Name { get; set; }
    public string Family { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ImageName { get; set; }
    public string NationalCode { get; set; }
    public string ActiveCode { get; set; }
    public string SecondActiveCode { get; set; }
    public string Presenter { get; set; }
    public int BranchId { get; set; }
    public DateTime LastSendActiveCodeDate { get; set; }
    public DateTime? BirthDate { get; set; }
    public UserGender Gender { get; set; }
    public bool IsActive { get; set; }
    public bool IsCompleteProfile { get; set; }


    #region Relations
    public ICollection<UserAddress> Addresses { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<UserNotification> UserNotifications { get; set; }
    public ICollection<Article> Articles { get; set; }
    public ICollection<Order> Orders { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
    public ICollection<Wallet> Wallets { get; set; }
    public ICollection<UserCard> UserCards { get; set; }
    public ICollection<UserPoint> Points { get; set; }
    #endregion
}
public enum UserGender
{
    آقا = 1,
    خانم = 2,
    نا_مشخص = 3
}
