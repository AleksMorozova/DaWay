using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using InstaBotLibrary.Integrator;
using InstaBotLibrary.TelegramBot;
using InstaBotLibrary.Bound;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaBotPrototype.Extensions
{
    public static class IntegratorExtensions
    {
        public static IWebHost StartIntegrator(this IWebHost webHost)
        {
            using (IServiceScope scope = webHost.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                ITelegramService telegramService = services.GetRequiredService<ITelegramService>();
                IBoundRepository repository = services.GetRequiredService<IBoundRepository>();
                IIntegratorFactory factory = services.GetRequiredService<IIntegratorFactory>();
                List<BoundModel> models = repository.getAllBounds();
                foreach (BoundModel model in models)
                {
                    if (model.InstagramToken != null && model.TelegramChatId != null)
                    {
                        IIntegrator integrator = factory.Create(model);
                        integrator.Start();
                    }
                }
                telegramService.Start();
            }
            return webHost;
        }
    }
}
