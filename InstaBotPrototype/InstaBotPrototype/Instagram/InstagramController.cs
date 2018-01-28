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
    [Route("[controller]/[action]")]
    public class InstagramController : Controller
    {
        InstagramConfig config;


        public InstagramController(IConfiguration configuration)
        {
            IConfigurationSection instagramAuthSection = configuration.GetSection("InstagramAuth");

            var clientId = instagramAuthSection["client_id"];
            var clientSecret = instagramAuthSection["client_secret"];
            var redirectUri = instagramAuthSection["redirect_uri"];
            var realtimeUri = "";
            config = new InstagramConfig(clientId, clientSecret, redirectUri, realtimeUri);
        }



        //Redirect to the link
        public RedirectResult Login()
        {
            var scopes = new List<OAuth.Scope>();
            scopes.Add(InstaSharp.OAuth.Scope.Basic);
            scopes.Add(InstaSharp.OAuth.Scope.Public_Content);

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
            return Redirect("http://localhost:58687/instagram/MyFeed");
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
            UserResponse userResponse = await users.GetSelf();

            users.OAuthResponse.User = userResponse.Data;

            var feed = users.RecentSelf();

            //return View(feed.Data);
            return Json(feed);
        }
    }
}