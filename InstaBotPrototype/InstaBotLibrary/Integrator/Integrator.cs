using System;
using System.Collections.Generic;
using InstaBotLibrary.Instagram;
using InstaBotLibrary.Telegram;
using Hangfire;
using InstaBotLibrary.Posts;

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




        public async void Update()
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
