using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using InstaBotLibrary.Instagram;
using InstaBotLibrary.Telegram;
using Hangfire;

namespace InstaBotLibrary.Integrator
{
    public class Telegr : ITelegramService
    {
        public int Connect(string username)
        {
            throw new NotImplementedException();
        }

        public string SendMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
    public class Integrator : IIntegrator
    {
        private IInstagramService instagramService;
        private ITelegramService telegramService;
        public Integrator(IInstagramService instagram, ITelegramService telegram)
        {
            instagramService = instagram;
            telegramService = telegram;
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
