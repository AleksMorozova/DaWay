using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InstaBotPrototype.UI.Controllers
{
    [Route("/")]
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        
    }
}