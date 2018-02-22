using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using InstaBotLibrary.Instagram;
using InstaBotLibrary.Telegram;
using Hangfire;
using InstaBotLibrary.Bound;

namespace InstaBotLibrary.Integrator
{
    public class Integrator : IIntegrator
    {
        private IInstagramService instagramService;
        private int boundId;

        public Integrator(IInstagramService instagram)
        {
            instagramService = instagram;
        }





        public event BotNotification SendMessage;

        
        public void Auth(BoundModel model)
        {
            boundId = model.Id;
            instagramService.Auth(model.InstagramToken, model.InstagramId);
        }


        [AutomaticRetry(Attempts = 0)]
        public async Task Update()
        {
            IEnumerable<Post> posts = await instagramService.GetLatestPosts();
            foreach (var post in posts)
            {
                //if (posts.containsFilters)
                //{
                //    telegramService.SendMessage(post);
                //}
                
            }

        }

        public void Start()
        {
            RecurringJob.AddOrUpdate(() => Update(), Cron.Minutely);
        }

    }
}
