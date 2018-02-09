﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using InstaSharp;
using InstaBotLibrary.Instagram;

namespace InstaBotPrototype
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.Configure<InstagramConfig>(Configuration.GetSection("InstagramSettings"));
            services.AddTransient<IInstagramService, InstagramService>();
            services
            .AddMvc()
            .AddRazorOptions(options => options.ViewLocationExpanders.Add(new ViewLocationExpander()));

            services.AddDistributedMemoryCache();
            services.AddSession();
            
             services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => 
            {
                options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/login");
            });

            services.AddTransient<IUserManager, UserManager>();
        }

       // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvc();
            app.UseAuthentication();
        }
    }
}
