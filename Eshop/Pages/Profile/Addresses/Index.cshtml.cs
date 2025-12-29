using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class IndexModel : PageUtil
    {
        private readonly IUserAddressesService _address;

        public IndexModel(IUserAddressesService address)
        {
            _address = address;
        }
        public List<UserAddress> UserAddresses { get; set; }
        public async Task OnGet()
        {
            UserAddresses = await _address.GetUserAddresses(User.GetUserId());
        }

        public async Task<IActionResult> OnGetDeleteAddress(long addressId)
        {
            return await AjaxTryCatch(async () =>
            {
                await _address.DeleteAddress(addressId, User.GetUserId());
            }, successReturn: "Deleted");
        }
        public async Task<IActionResult> OnGetActiveAddress(long addressId)
        {
            return await AjaxTryCatch(async () =>
            {
                await _address.SetAddressToActive(addressId, User.GetUserId());
            }, successReturn: "Success");
        }
        
    }
}
