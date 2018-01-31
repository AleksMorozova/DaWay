using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using InstaBotLibrary.User;
using InstaBotLibrary.Bound;
using InstaBotLibrary.Filter;

namespace InstaBotPrototype.UI.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [Route("/")]
        [HttpGet]
         public ActionResult Index()
         {
            //HttpContext.Session.SetInt32("user_id", 2);
            bool aval = HttpContext.Session.IsAvailable;
            int? id = HttpContext.Session.GetInt32("user_id");
            if (aval && id != null)
            {
                //return Content("User id is " + HttpContext.Session.GetInt32("user_id"));
                return Redirect("/home");
            }
            return Redirect("/login");
         }
		 


        [Route("/home")]
        public IActionResult Home()
        {
            int id = HttpContext.Session.GetInt32("user_id").Value;
            UserRepository userRepository = new UserRepository();
            UserModel user = userRepository.getUserInfo(id);
            BoundRepository boundRepository = new BoundRepository();
            BoundModel bound = boundRepository.getFirstOrCreateUserBound(id);
            List<FilterModel> filters = (new FilterRepository()).getBoundFilters(bound.Id);
            ViewBag.bound = bound;
            ViewBag.user = user;
            ViewBag.filters = filters;


            return View();
        }




        [Route("/exit")]
        public IActionResult Exit()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}
