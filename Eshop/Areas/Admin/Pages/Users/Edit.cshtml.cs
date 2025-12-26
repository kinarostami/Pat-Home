using CoreLayer.DTOs.Admin.Users;
using CoreLayer.Services.Users;
using Common.Application.SecurityUtil;
using CoreLayer.Utilities;
using DomainLayer.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DomainLayer.Models.Roles;

namespace Eshop.Areas.Admin.Pages.Users
{
    //[PermissionChecker(Permissions.مدیریت_کاربران)]

    [ValidateAntiForgeryToken]
    public class EditModel : PageModel
    {
        private readonly IUserService _user;

        public EditModel(IUserService user)
        {
            _user = user;
        }
        [BindProperty]
        public EditUserViewModel UserModel { get; set; }
        public async Task<IActionResult> OnGet(long userId)
        {
            var user = await _user.GetSingleUser(userId,true);
            if (user == null) return RedirectToPage("Index");
            InitData(user);
            return Page();
        }

        public async Task<IActionResult> OnPost(long userId)
        {
            if (UserModel.UserImage != null && !UserModel.UserImage.IsImage())
            {
                ModelState.AddModelError("UserModel.UserImage", "لطفا عکس وارد کنید");
                return Page();
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            UserModel.Id = userId;
            var res = await _user.EditUser(UserModel);
            if (!res)
            {
                TempData["Error"] = true;
                return Page();
            }
            TempData["Success"] = true;
            return RedirectToPage("Index");
        }


        private void InitData(User user)
        {
            UserModel = new EditUserViewModel()
            {
                Family = user.Family,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                UserRoles = user.UserRoles.Select(s => s.RoleId).ToList(),
                Email = user.Email,
                ImageName = user.ImageName,
                IsActive = user.IsActive
            };

        }
    }
}
