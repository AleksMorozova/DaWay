
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using InstaBotLibrary.Instagram;
using InstaSharp.Models.Responses;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using InstaBotLibrary.Bound;

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
            if (HttpContext.Session.GetInt32("user_id") == null)
            {
                return Redirect("/");
            }

            string link = instagramService.getLoginLink();

            return Redirect(link);
        }

        //Getting the access_token
        public async Task<ActionResult> OAuth(string code)
        {
            string token = await instagramService.GetToken(code);

            BoundRepository repo = new BoundRepository();
            int userId = HttpContext.Session.GetInt32("user_id").Value;
            BoundModel bound = repo.getFirstOrCreateUserBound(userId);
            bound.InstagramToken = token;
            repo.SetInstagramToken(bound);
            //repo.UpdateBound(bound);
            //HttpContext.Session.SetString("InstaSharp.AuthInfo", token);

            //return RedirectToAction("Index");
            return Redirect("http://localhost:58687/instagram/MyFeed");
        }


        public async Task<ActionResult> MyFeed()
        {
            //string token = HttpContext.Session.GetString("InstaSharp.AuthInfo");
            BoundRepository repo = new BoundRepository();
            int userId = HttpContext.Session.GetInt32("user_id").Value;
            BoundModel bound = repo.getFirstOrCreateUserBound(userId);
            string token = bound.InstagramToken;

            if (token == null)
            {
                return Redirect("/login");
            }

            MediasResponse feed = await instagramService.GetMedias(token);

            //return View(feed.Data);
            return Json(feed);
        }
    }
}