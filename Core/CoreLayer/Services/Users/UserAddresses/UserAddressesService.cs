using Common.Application;
using Common.Application.SecurityUtil;
using CoreLayer.DTOs.Profile;
using DataLayer.Context;
using DomainLayer.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.Services.Users.UserAddresses;

public class UserAddressesService : BaseService, IUserAddressesService
{
    public UserAddressesService(AppDbContext context) : base(context)
    {
    }

    public async Task AddAddress(AddressViewModel addressModel)
    {
        if (addressModel.NationalCode.IsText()) return;
        if (addressModel.Phone.IsText()) return;
        if (addressModel.PostalCode.IsText()) return;
        var isActive = !await _context.UserAddresses.AnyAsync(c => c.IsActive && c.UserId == addressModel.UserId);
        var address = new UserAddress()
        {
            City = addressModel.City.SanitizeText(),
            Family = addressModel.Family.SanitizeText(),
            Address = addressModel.Address.SanitizeText(),
            UserId = addressModel.UserId,
            PostalCode = addressModel.PostalCode.SanitizeText(),
            NationalCode = addressModel.NationalCode.SanitizeText(),
            Name = addressModel.Name.SanitizeText(),
            Shire = addressModel.Shire.SanitizeText(),
            Phone = addressModel.Phone.SanitizeText(),
            IsActive = isActive
        };
        Insert(address);
        await Save();
    }

    public async Task<bool> DeleteAddress(long addressId)
    {
        var address = await GetById<UserAddress>(addressId);
        if (address == null) return false;
        Delete(address);
        await Save();
        return true;
    }

    public async Task<bool> DeleteAddress(long addressId, long userId)
    {
        var address = await _context.UserAddresses.SingleOrDefaultAsync(a => a.Id == addressId && a.UserId == userId);
        if (address == null) return false;
        Delete(address);
        await Save();
        return true;
    }

    public async Task EditAddress(AddressViewModel addressModel)
    {
        if (addressModel.NationalCode.IsText()) return;
        if (addressModel.Phone.IsText()) return;
        if (addressModel.PostalCode.IsText()) return;

        var address = await GetById<UserAddress>(addressModel.Id);
        if (address == null)
            return;

        address.Name = addressModel.Name.SanitizeText();
        address.Family = addressModel.Family.SanitizeText();
        address.NationalCode = addressModel.NationalCode.SanitizeText();
        address.Phone = addressModel.Phone.SanitizeText();
        address.PostalCode = addressModel.PostalCode.SanitizeText();
        address.Shire = addressModel.Shire.SanitizeText();
        address.UserId = addressModel.UserId;
        address.City = addressModel.City.SanitizeText();
        address.Address = addressModel.Address.SanitizeText();
        Update(address);
        await Save();
    }

    public async Task<UserAddress> GetActiveAddress(long userId)
    {
        return await Table<UserAddress>().SingleOrDefaultAsync(a => a.IsActive && a.UserId == userId);
    }

    public async Task<UserAddress> GetUserAddress(long addressId)
    {
        return await GetById<UserAddress>(addressId);
    }

    public async Task<UserAddress> GetUserAddress(long addressId, long userId)
    {
        return await _context.UserAddresses.AsNoTracking()
                .SingleOrDefaultAsync(a => a.Id == addressId && a.UserId == userId);
    }

    public async Task<List<UserAddress>> GetUserAddresses(long userId)
    {
        return await Table<UserAddress>()
                .Where(a => a.UserId == userId).OrderByDescending(c => c.IsActive).ToListAsync();
    }

    public async Task SetAddressToActive(long addressId, long userId)
    {
        var userAddresses = Table<UserAddress>().Where(a => a.UserId == userId);
        if (userAddresses.Any(a => a.Id == addressId && a.UserId == userId && !a.IsActive))
        {
            //آدرس های فعال را غیر فعال میکنیم
            foreach (var item in userAddresses.Where(a => a.IsActive))
            {
                item.IsActive = false;
                Update(item);
            }
            //آدرس انتخابی را فعال میکنم
            var address = await userAddresses.SingleOrDefaultAsync(a => a.Id == addressId && a.UserId == userId);
            address.IsActive = true;
            Update(address);
            await Save();
        }
    }
}
