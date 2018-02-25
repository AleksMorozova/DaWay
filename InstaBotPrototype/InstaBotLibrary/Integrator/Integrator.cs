using System;
using System.Threading.Tasks;
using System.Timers;
using System.Collections.Generic;
using InstaBotLibrary.Instagram;
using InstaBotLibrary.FilterManager;
using Hangfire;
using InstaBotLibrary.Bound;

namespace InstaBotLibrary.Integrator
{
    public class Integrator : IIntegrator
    {
        private IInstagramService instagramService;
        private TagsProcessor tagsProcessor;
        private int boundId;

        public Integrator(IInstagramService instagram, TagsProcessor processor)
        {
            instagramService = instagram;
            tagsProcessor = processor;
        }





        public event BotNotification SendPost;

        
        public void Auth(BoundModel model)
        {
            boundId = model.Id;
            instagramService.Auth(model.InstagramToken, model.InstagramId.Value);
        }


        [AutomaticRetry(Attempts = 0)]
        public async Task Update()
        {
            IEnumerable<Post> posts = await instagramService.GetLatestPosts(boundId);
            foreach (var post in posts)
            {
                if (await tagsProcessor.TagIntersectionAsync(post, boundId))
                {
                    SendPost?.Invoke(boundId, post);
                }
            }
        }


        public void Start()
        {
            Timer timer = new Timer(30000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            Update();
            //RecurringJob.AddOrUpdate(() => Update(), Cron.MinuteInterval(1));
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Update();
        }
    }
}
