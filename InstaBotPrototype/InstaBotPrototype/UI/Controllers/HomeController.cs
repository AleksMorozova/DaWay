using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InstaBotPrototype.UI.Controllers
{
    [Route("/home")]
    public class HomeController : Controller
    {
        [HttpGet]
         public ActionResult Index()
         {
            //HttpContext.Session.SetInt32("user_id", 2);
            bool aval = HttpContext.Session.IsAvailable;
            int? id = HttpContext.Session.GetInt32("user_id");
            if (aval && id != null)
            {
                return Content("User id is " + HttpContext.Session.GetInt32("user_id"));
            }
            return Redirect("/login");
         }
		 
        public IActionResult Home()
        {
            return View();
        }
    }
}
