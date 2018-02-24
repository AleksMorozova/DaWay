using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using InstaBotLibrary.Instagram;
using System.Threading.Tasks;
using InstaBotLibrary.Bound;
using InstaSharp.Models.Responses;

namespace InstaBotPrototype.Instagram
{
    [Route("[controller]/[action]")]
    public class InstagramController : Controller
    {
        private IInstagramService instagramService;
        private IBoundRepository boundRepository;

        public InstagramController(IInstagramService service, IBoundRepository boundRepository)
        {
            instagramService = service;
            this.boundRepository = boundRepository;
        }


        public RedirectResult Login(string token)
        {
            string link;
            if (token != null)
                link = instagramService.getLoginLink("temp_token", token);
            else link = instagramService.getLoginLink();

            return Redirect(link);
        }

        //Getting the access_token
        public async Task<ActionResult> OAuth(string temp_token, string code)
        {
            OAuthResponse response = await instagramService.GetResponse(code);
            if (temp_token == null)
            {
                //add token to logged in user
            }
            else
            {
                BoundModel bound = boundRepository.GetBoundByTempToken(temp_token);
                bound.InstagramToken = response.AccessToken;
                bound.InstagramId = (int)response.User.Id;
                boundRepository.UpdateBound(bound);
            }

            return Redirect("http://localhost:58687/");
        }
    }
}