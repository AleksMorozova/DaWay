using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using InstaBotLibrary.Instagram;
using System.Threading.Tasks;
using InstaBotLibrary.TelegramBot;

namespace InstaBotPrototype.Instagram
{
    [Route("[controller]/[action]")]
    public class InstagramController : Controller
    {
        private IInstagramService instagramService;

        public InstagramController(IInstagramService service)
        {
            instagramService = service;
        }


        public RedirectResult Login(string token)
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