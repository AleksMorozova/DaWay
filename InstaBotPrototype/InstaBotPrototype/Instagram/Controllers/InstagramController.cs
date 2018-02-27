using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using InstaBotLibrary.Instagram;
using System.Threading.Tasks;
using InstaBotLibrary.Bound;
using InstaSharp.Models.Responses;
using InstaBotLibrary.Integrator;

namespace InstaBotPrototype.Instagram
{
    [Route("[controller]/[action]")]
    public class InstagramController : Controller
    {
        private IInstagramService instagramService;
        private IBoundRepository boundRepository;
        IIntegratorFactory integratorFactory;

        public InstagramController(IInstagramService service, IBoundRepository boundRepository, IIntegratorFactory factory)
        {
            instagramService = service;
            this.boundRepository = boundRepository;
            integratorFactory = factory;
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
            OAuthResponse response = await instagramService.GetResponse(temp_token, code);
            if (temp_token == null)
            {
                //add token to logged in user
            }
            else
            {
                BoundModel bound = boundRepository.GetBoundByTempToken(temp_token);
                bound.InstagramToken = response.AccessToken;
                bound.InstagramId = response.User.Id;
                boundRepository.UpdateBound(bound);
                IIntegrator integrator = integratorFactory.Create(bound);
                integrator.Start();
            }

            return Redirect("http://localhost:58687/");
        }
    }
}