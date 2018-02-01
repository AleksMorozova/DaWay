
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using InstaBotLibrary.Instagram;
using InstaSharp.Models.Responses;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace InstaBotPrototype.Instagram
{
    [Route("[controller]/[action]")]
    public class InstagramController : Controller
    {
        InstagramService instagramService;

        public InstagramController(IConfiguration configuration)
        {
            instagramService = new InstagramService(configuration);
            
        }


        public RedirectResult Login()
        {
            string link = instagramService.getLoginLink();

            return Redirect(link);
        }

        //Getting the access_token
        public async Task<ActionResult> OAuth(string code)
        {
            string token = await instagramService.GetToken(code);

            HttpContext.Session.SetString("InstaSharp.AuthInfo", token);

            //return RedirectToAction("Index");
            return Redirect("http://localhost:58687/instagram/MyFeed");
        }


        public async Task<ActionResult> MyFeed()
        {
            string token = HttpContext.Session.GetString("InstaSharp.AuthInfo");
            if (token == null)
            {
                return RedirectToAction("Login");
            }

            MediasResponse feed = await instagramService.GetMedias(token);

            //return View(feed.Data);
            return Json(feed);
        }
    }
}