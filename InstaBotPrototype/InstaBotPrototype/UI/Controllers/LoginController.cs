using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InstaBotLibrary.User;
using InstaBotLibrary.Authorization;

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
            string [] strarr = {"", "Incorrect login", "Incorrect password"};
            ViewBag.message = strarr[errortype.GetValueOrDefault(0)];
            return View();
        }


        [HttpPost]
        public ActionResult Authorize(string login, string password)
        {
            UserRepository repo = new UserRepository();
            AuthorizationModel auth = new AuthorizationModel();
            auth = repo.getUserAuthorizationInfo(login);
            if (auth == null)
            {
                return Redirect("/login/?errortype=1");
            } 
            else
            {
                if (auth.Password != password)
                {
                    return Redirect("/login/?errortype=2");
                }
                else
                {
                    HttpContext.Session.SetInt32("user_id", auth.Id);
                    return Redirect("/home");
                }
            }
        }

    }
}