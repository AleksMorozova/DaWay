using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using InstaBotLibrary.Integrator;
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

                IIntegrator integrator = services.GetRequiredService<IIntegrator>();
                integrator.Start();
            }
            return webHost;
        }
    }
}
