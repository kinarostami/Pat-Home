using CoreLayer.Utilities;
using DomainLayer.Models.Roles;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Areas.Admin.Pages.Users
{
    [PermissionChecker(Permissions.مدیریت_کاربران)]

    public class AddModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
