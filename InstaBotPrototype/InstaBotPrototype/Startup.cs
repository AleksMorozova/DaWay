using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using InstaSharp;
using InstaBotLibrary.Instagram;
using Microsoft.AspNetCore.Authentication.Cookies;
using InstaBotPrototype.Services;
using InstaBotLibrary.User;
using InstaBotLibrary.AI;
using InstaBotLibrary.Tokens;
using InstaBotLibrary.Integrator;
using InstaBotLibrary.Bound;
using InstaBotLibrary.DbCommunication;
using InstaBotLibrary.FilterManager;
using InstaBotLibrary.Filter;
using InstaBotLibrary.TelegramBot;


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
            services.Configure<MicrosoftVisionOptions>(Configuration.GetSection("MicrosoftVisionApi"));
            services.Configure<DbConnectionOptions>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<TelegramBotOptions>(Configuration.GetSection("TelegramBotSettings"));
            services.AddSingleton<IRecognizer, MicrosoftImageRecognizer>();
            services.AddTransient<TagsProcessor>();
            services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();
            services.AddTransient<IBoundRepository, BoundRepository>();
            services.AddTransient<IInstagramService, InstagramService>();
            services.AddTransient<IFilterRepository, FilterRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITokenGenerator, TokenGenerator>();
            services.AddTransient<IIntegrator, Integrator>();
            services.AddSingleton<ITelegramService, TelegramBot>();
            services.AddTransient<IIntegratorFactory, IntegratorFactory>();


			
            services
            .AddMvc()
            .AddRazorOptions(options => options.ViewLocationExpanders.Add(new ViewLocationExpander()));
            
             services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => 
            {
                options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/login");
            });

            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRecognizer, MicrosoftImageRecognizer>();
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
