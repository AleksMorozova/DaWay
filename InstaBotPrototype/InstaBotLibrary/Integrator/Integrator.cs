using System;
using System.Collections.Generic;
using InstaBotLibrary.Instagram;
using InstaBotLibrary.Telegram;
using Hangfire;

namespace InstaBotLibrary.Integrator
{
    public class Integrator
    {
        private IInstagramService instagramService;
        private ITelegramService telegramService;
        public Integrator(IInstagramService instagram, ITelegramService telegram)
        {
            instagramService = instagram;
            telegramService = telegram;
        }




        public void Update()
        {
            IEnumerable<string> posts = instagramService.GetLatestPosts();
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
