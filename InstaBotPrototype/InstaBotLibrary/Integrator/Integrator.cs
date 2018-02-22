using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using InstaBotLibrary.Instagram;
using InstaBotLibrary.Telegram;
using Hangfire;

namespace InstaBotLibrary.Integrator
{
    public class Integrator : IIntegrator
    {
        private IInstagramService instagramService;

        public event BotNotification SendMessage;

        public Integrator(IInstagramService instagram)
        {
            instagramService = instagram;
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
