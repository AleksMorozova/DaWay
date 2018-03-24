using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InstaBotPrototype.UI.Controllers
{
    [Route("/home")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Home()
        {
            if (User.Identity.IsAuthenticated)
                return View();
            else
                return Redirect("/login");
        }

        [Route("/")]
        public RedirectResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return Redirect("/home");
            else
                return Redirect("/login");
        }

        [Route("/exit")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectPermanent("/login");
        }
    }
}
