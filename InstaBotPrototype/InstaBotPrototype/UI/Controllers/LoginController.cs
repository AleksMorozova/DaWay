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
    [Route("/")]
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Login(int? errortype)
        {
            string [] strarr = {"", "Incorrect login", "Incorrect password"};
            ViewBag.message = strarr[errortype.GetValueOrDefault(0)];
            return View();
        }

        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            UserRepository repo = new UserRepository();
            AuthorizationModel auth = new AuthorizationModel();
            auth = repo.getUserAuthorizationInfo(login);
            if (auth == null)
            {
                return Redirect("/?errortype=1");
            } 
            else
            {
                if (auth.Password != password)
                {
                    return Redirect("/?errortype=2");
                }
                else
                {
                    return Redirect("/home");
                }
            }
        }

    }
}