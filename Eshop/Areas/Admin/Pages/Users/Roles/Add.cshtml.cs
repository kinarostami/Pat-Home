using System.Collections.Generic;
using System.Threading.Tasks;
using CoreLayer.Services.Users;
using Common.Application.SecurityUtil;
using CoreLayer.Utilities;
using DomainLayer.Enums;
using DomainLayer.Models.Roles;
using Eshop.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Areas.Admin.Pages.Users.Roles
{
    [PermissionChecker(Permissions.مدیریت_نقش_ها)]

    [ValidateAntiForgeryToken]
    public class AddModel : PageUtil
    {
        private readonly IUserRoleService _role;

        public AddModel(IUserRoleService role)
        {
            _role = role;
        }
        [BindProperty]
        public Role Role { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(List<Permissions> permissions)
        {
            return await TryCatch(async () =>
            {
                await _role.AddRole(Role, permissions);
            }, successReturn: "/Admin/Users/Roles");
        }
    }
}
