using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eshop.Pages.Payment
{
    public class CheckOutModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (TempData["Success"] == null && TempData["Error"] == null)
                return Redirect("/");
            return Page();
        }
    }
}
