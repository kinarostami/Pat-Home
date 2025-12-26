using System.Collections.Generic;
using System.Threading.Tasks;
using CoreLayer.Services.Users;
using Common.Application.SecurityUtil;
using CoreLayer.Utilities;
using DomainLayer.Enums;
using DomainLayer.Models.Roles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Areas.Admin.Pages.Users.Roles
{
    [PermissionChecker(Permissions.مدیریت_نقش_ها)]
    public class IndexModel : PageModel
    {
        private readonly IUserRoleService _role;

        public IndexModel(IUserRoleService role)
        {
            _role = role;
        }
        public List<Role> Roles { get; set; }
        public async Task OnGet()
        {
            Roles =await _role.GetAllRole();
        }

        public async Task<IActionResult> OnGetDeleteRole(long roleId)
        {
            try
            {
                var res =await _role.DeleteRole(roleId);
                return Content(res ? "Deleted" : "Error");
            }
            catch
            {
                return Content("Error");
            }
        }
    }
}
