using System.Threading.Tasks;
using CoreLayer.Services.Users;
using DomainLayer.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Areas.Admin.Pages.Users
{
    public class ShowModel : PageModel
    {
        private readonly IUserService _userService;

        public ShowModel(IUserService userService)
        {
            _userService = userService;
        }
        public User UserModel { get; set; }
        public async Task<IActionResult> OnGet(long userId)
        {
            UserModel = await _userService.GetSingleUser(userId, true);
            if (UserModel == null) return RedirectToPage("Index");

            return Page();
        }
    }
}
