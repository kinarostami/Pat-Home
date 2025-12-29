using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLayer.DTOs.Profile;
using CoreLayer.Services.Users.UserAddresses;
using Common.Application.UserUtil;
using DomainLayer.Models.Users;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Pages.Profile.Addresses
{
    [Authorize]
    [ValidateAntiForgeryToken]
    public class EditModel : PageUtil
    {
        private readonly IUserAddressesService _address;

        public EditModel(IUserAddressesService address)
        {
            _address = address;
        }
        [BindProperty]
        public AddressViewModel Address { get; set; }
        public async Task<IActionResult> OnGet(long addressId)
        {
            var address = await _address.GetUserAddress(addressId, User.GetUserId());
            if (address == null)
                return RedirectToPage("Index");

            Address = new AddressViewModel()
            {
                Address = address.Address,
                City = address.City,
                Family = address.Family,
                NationalCode = address.NationalCode,
                Name = address.Name,
                Phone = address.Phone,
                PostalCode = address.PostalCode,
                Shire = address.Shire
            };
            return Page();
        }

        public async Task<IActionResult> OnPost(long addressId)
        {
            return await TryCatch(async () =>
            {
                Address.UserId = User.GetUserId();
                Address.Id = addressId;
                await _address.EditAddress(Address);
            }, successReturn: "/Profile/Addresses", errorReturn: $"/Profile/Addresses/Edit/{addressId}");
        }
    }
}
