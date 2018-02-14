using System;
using System.Collections.Generic;
using System.Timers;
using InstaBotLibrary.Instagram;
using InstaBotLibrary.Telegram;

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
            int munutesInterval = 1;
            Timer timer = new Timer(60000 * munutesInterval);
            timer.AutoReset = true;
            timer.Elapsed += (a, b) => Update();
        }

    }
}
