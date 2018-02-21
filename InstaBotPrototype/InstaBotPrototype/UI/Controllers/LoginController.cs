using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using InstaBotPrototype.Services;

namespace InstaBotPrototype.UI.Controllers
{
    [Route("login/[action]")]
    public class LoginController : Controller
    {
        // GET: Login
        [Route("/login")]
        [HttpGet]
        public ActionResult Login(int? errortype)
        {
            ViewBag.message = TempData["message"];
            return View();
        }
        
        private IUserManager userManager;

        public LoginController(IUserManager manager)
        {
            userManager = manager;
        }

        [Route("/auth")]
        [HttpPost]
        public async Task<ActionResult> AuthAsync(string login, string password)
        {
            if (ModelState.IsValid && userManager.IsLoggedIn(login, password))
            {
                //HttpContext.Session.SetInt32("user_id", userManager.SessionId(login));
                await Authenticate(login);
                return RedirectPermanent("/home");
            }
            else
            {
                TempData["message"] = "Incorrect login or password";
                return RedirectPermanent("/login");
            }
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
