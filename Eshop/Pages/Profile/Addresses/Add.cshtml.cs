using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLayer.DTOs.Profile;
using CoreLayer.Services.Users.UserAddresses;
using Common.Application.UserUtil;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Pages.Profile.Addresses
{
    [Authorize]
    [ValidateAntiForgeryToken]
    public class AddModel : PageUtil
    {
        private readonly IUserAddressesService _address;

        public AddModel(IUserAddressesService address)
        {
            _address = address;
        }
        [BindProperty]
        public AddressViewModel Address { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            return await TryCatch(async () =>
            {
                Address.UserId = User.GetUserId();
                await _address.AddAddress(Address);
            }, successReturn: "/Profile/Addresses");
        }
    }
}
