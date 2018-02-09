using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using InstaBotLibrary.Instagram;
using System.Threading.Tasks;

namespace InstaBotPrototype.Instagram
{
    [Route("[controller]/[action]")]
    public class InstagramController : Controller
    {
        private InstagramService instagramService;

        public InstagramController(InstagramService service)
        {
            instagramService = service;
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

            //HERE WE GET AND CAN SAVE TOKEN

            return Redirect("http://localhost:58687/");
        }
    }
}