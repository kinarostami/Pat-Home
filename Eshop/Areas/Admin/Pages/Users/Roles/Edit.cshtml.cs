using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLayer.Services.Users;
using Common.Application.SecurityUtil;
using CoreLayer.Utilities;
using DomainLayer.Enums;
using DomainLayer.Models.Roles;
using Eshop.Infrastructure;
using Eshop.Static;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Areas.Admin.Pages.Users.Roles
{
    [PermissionChecker(DomainLayer.Enums.Permissions.مدیریت_نقش_ها)]

    public class EditModel : PageUtil
    {
        private readonly IUserRoleService _role;

        public EditModel(IUserRoleService role)
        {
            _role = role;
        }
        [BindProperty]

        public Role Role { get; set; }
        [BindProperty]
        public List<Permissions> Permissions { get; set; }
        public async Task<IActionResult> OnGet(long roleId)
        {
            await InitData(roleId);
            if (Role == null) return RedirectToPage("Index");
            return Page();
        }

        public async Task<IActionResult> OnPost(long roleId)
        {
            var role = await _role.GetRoleNoTask(roleId);
            if (!isRoleShouldUpdate(role, Role.RoleTitle))
            {
                Role = role;
                Permissions = role.RolePermissions.Select(p => p.PermissionId).ToList();
                TempData["Error"] = "True";
                ModelState.AddModelError("Role.RoleTitle", "امکان تغییر عنوان این نقش وجود ندارد، شما فقط قادر به تغییر دسترسی هستید");
                return Page();
            }

            return await TryCatch(async () =>
            {
                Role.Id = roleId;
                await _role.EditRole(Role, Permissions);
                AppStatic.RolePermissions = null;
            }, successReturn: "/Admin/Users/Roles", errorReturn: $"/Admin/Users/Roles/Edit/{roleId}");
        }

        private async Task InitData(long roleId)
        {
            var role = await _role.GetRoleById(roleId);
            Role = role;
            Permissions = role.RolePermissions.Select(p => p.PermissionId).ToList();
        }

        private bool isRoleShouldUpdate(Role role, string newRoleTitle)
        {
            switch (role.RoleTitle)
            {
                case "ادمین" when newRoleTitle.Trim() != "ادمین":
                case "کاربر" when newRoleTitle.Trim() != "کاربر":
                case "فروشنده" when newRoleTitle.Trim() != "فروشنده":
                    return false;
                default:
                    return true;
            }
        }
    }
}
