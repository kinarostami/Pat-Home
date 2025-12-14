using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using CoreLayer.Services;
using Microsoft.AspNetCore.Hosting;

namespace Eshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private IAppContext _appContext;

        public HomeController(IWebHostEnvironment env, IAppContext appContext)
        {
            _env = env;
            _appContext = appContext;
        }

        [Route("/HandlerError/{code}")]
        public IActionResult HandlerStatusCode(int code)
        {
            if (code == 404)
            {
                return View("NotFound");

            }

            return Redirect("/NotFound");
        }
    }
}
