using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.Users;

public class UserAddress : BaseEntity
{
    public long UserId { get; set; }
    public string PostalCode { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Shire { get; set; }
    public string Name { get; set; }
    public string Family { get; set; }
    public string Phone { get; set; }
    public string NationalCode { get; set; }
    public bool IsActive { get; set; }


    public User User { get; set; }
}
