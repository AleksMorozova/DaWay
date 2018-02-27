using System;
using System.Collections.Generic;
using System.Text;
using InstaBotLibrary.Bound;
using InstaBotLibrary.TelegramBot;
using InstaSharp;
using InstaBotLibrary.FilterManager;
using Microsoft.Extensions.Options;
using InstaBotLibrary.Instagram;

namespace InstaBotLibrary.Integrator
{
    public class IntegratorFactory : IIntegratorFactory
    {
        private ITelegramService telegram;
        private IOptions<InstagramConfig> config;
        private IBoundRepository repository;
        private TagsProcessor processor;
        public IntegratorFactory(ITelegramService telegram, IOptions<InstagramConfig> config, IBoundRepository boundRepository, TagsProcessor processor)
        {
            this.telegram = telegram;
            this.config = config;
            repository = boundRepository;
            this.processor = processor;
        }



        public IIntegrator Create(BoundModel model)
        {
            IInstagramService instagram = new InstagramService(config, repository);
            Integrator integrator = new Integrator(instagram, processor);
            integrator.SendPost += telegram.SendPost;
            integrator.Auth(model);
            return integrator;
        }
    }
}
