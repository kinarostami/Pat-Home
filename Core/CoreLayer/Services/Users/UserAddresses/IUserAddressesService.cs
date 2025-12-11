using CoreLayer.DTOs.Profile;
using DomainLayer.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Users.UserAddresses;

public interface IUserAddressesService
{
    Task<List<UserAddress>> GetUserAddresses(long userId);
    Task<UserAddress> GetUserAddress(long addressId);
    Task<UserAddress> GetActiveAddress(long userId);
    Task<UserAddress> GetUserAddress(long addressId, long userId);
    Task<bool> DeleteAddress(long addressId);
    Task<bool> DeleteAddress(long addressId, long userId);
    Task AddAddress(AddressViewModel addressModel);
    Task EditAddress(AddressViewModel addressModel);
    Task SetAddressToActive(long addressId, long userId);
}
