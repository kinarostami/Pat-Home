using CoreLayer.DTOs.Admin.Users;
using CoreLayer.Services.Users;
using CoreLayer.Utilities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages.Users
{
    [PermissionChecker(Permissions.مدیریت_کاربران)]

    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }
        public UsersViewModel UsersModel { get; set; }
        public async Task OnGet(int pageId=1,long userId=0,string email="",string nameOrFamily="",string phone="")
        {
            UsersModel =await _userService.GetUsersForAdmin(pageId, 15, email, nameOrFamily, userId,phone);
        }
    }
}
