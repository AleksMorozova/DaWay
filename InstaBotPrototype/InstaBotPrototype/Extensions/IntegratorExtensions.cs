using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using InstaBotLibrary.Integrator;
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
                IBoundRepository repository = services.GetRequiredService<IBoundRepository>();
                List<BoundModel> models = repository.getAllBounds();
                foreach (BoundModel model in models)
                {
                    if (model.InstagramToken != null)
                    {
                        IIntegrator integrator = services.GetRequiredService<IIntegrator>();
                        integrator.Auth(model);
                        //TODO: Add telegram subsciption
                        integrator.Start();
                    }
                }
                
            }
            return webHost;
        }
    }
}
