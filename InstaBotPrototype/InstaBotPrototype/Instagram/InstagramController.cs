using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using InstaSharp.Endpoints;
using InstaSharp.Models;
using InstaSharp.Models.Responses;
using InstaSharp;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace InstaBotPrototype.Instagram
{
    [Route("api/[controller]/[action]")]
    public class InstagramController : Controller
    {
        InstagramConfig config;


        public InstagramController(IConfiguration configuration)
        {
            //HttpContext.Session
            IConfigurationSection section = configuration.GetSection("InstagramAuth");

            var clientId = section["client_id"];
            var clientSecret = section["client_secret"];
            var redirectUri = section["redirect_uri"];
            var realtimeUri = "";
            config = new InstagramConfig(clientId, clientSecret, redirectUri, realtimeUri);
        }


        [HttpGet]
        public IActionResult Index()
        {
            //Users users = new Users(config, new OAuthResponse() { AccessToken = "asdas", User = new UserInfo() });
            //users.RecentSelf();
            return View();
        }

        //Redirect to the link
        //[HttpGet("{id}")]
        public RedirectResult Login()
        {
            var scopes = new List<OAuth.Scope>();
            scopes.Add(InstaSharp.OAuth.Scope.Basic);
            //scopes.Add(InstaSharp.OAuth.Scope.Likes);
            //scopes.Add(InstaSharp.OAuth.Scope.Comments);

            string link = InstaSharp.OAuth.AuthLink(config.OAuthUri + "authorize", config.ClientId, config.RedirectUri, scopes, InstaSharp.OAuth.ResponseType.Code);

            return Redirect(link);

        }

        //Getting the access_token
        public async Task<ActionResult> OAuth(string code)
        {

            var auth = new OAuth(config);

            var oauthResponse = await auth.RequestToken(code);

            HttpContext.Session.SetString("InstaSharp.AuthInfo", oauthResponse.AccessToken);

            //return RedirectToAction("Index");
            return Redirect("http://localhost:58687/api/instagram/MyFeed");
        }
        public async Task<ActionResult> MyFeed()
        {
            string token = HttpContext.Session.GetString("InstaSharp.AuthInfo");

            var oAuthResponse = new OAuthResponse() { AccessToken = token, User = new UserInfo()};

            if (oAuthResponse == null)
            {
                return RedirectToAction("Login");
            }
            
            var users = new Users(config, oAuthResponse);
            UserResponse user = await users.GetSelf();
            users.OAuthResponse.User.Id = user.Data.Id;
            var feed = users.RecentSelf();

            //return View(feed.Data);
            return Json(feed);
        }
    }
}