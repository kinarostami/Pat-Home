using System;
using System.Threading.Tasks;
using Common.Application.EmailUtil;
using CoreLayer.Services.Newsletters;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly INewsletterService _newsletter;

        public NewsletterController(INewsletterService newsletter)
        {
            _newsletter = newsletter;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterMember(string email)
        {
            if (!email.IsValidEmail())
            {
                TempData["Error"] = true;
                var redirect = Request.Headers["Referer"].ToString();
                return Redirect(redirect);
            }

            var res = await _newsletter.RegisterToNewsLetter(email);
            if (res)
            {
                TempData["SuccessRegister"] = true;
            }
            else
            {
                TempData["SuccessRegister"] = false;
            }
            var redirectPath = Request.Headers["Referer"].ToString();
            return Redirect(redirectPath);
        }
        [Route("/Newsletter/RemoveMember/{code}/{email}")]
        public async Task<IActionResult> RemoveMember(Guid code,string email)
        {
            var res = await _newsletter.DeleteFromNewsLetter(email,code);
            if (res)
            {
                TempData["SuccessDeleted"] = true;
            }
            else
            {
                TempData["SuccessDeleted"] = false;
            }
            return Redirect("/");
        }
    }
}