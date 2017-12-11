using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tweets.Web.Models;
using Tweets.Web.Services;

namespace Tweets.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/login";
            })
            .AddTwitter(twitterOptions =>
            {
                twitterOptions.ConsumerKey = Configuration["Authentication:Twitter:ConsumerKey"];
                twitterOptions.ConsumerSecret = Configuration["Authentication:Twitter:ConsumerSecret"];
                twitterOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            //Configure MVC in services collection
            services.AddMvc();

            //Inject Services
            services.AddTransient<IHttpHelper, HttpHelper>();
            services.AddTwitterServices( new TwitterServiceOptions
            {
                oauth_token = Configuration["Authentication:Twitter:oauth_token"],
                oauth_token_secret = Configuration["Authentication:Twitter:oauth_token_secret"],
                oauth_consumer_key = Configuration["Authentication:Twitter:ConsumerKey"],
                oauth_consumer_secret = Configuration["Authentication:Twitter:ConsumerSecret"]
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Allow serving static files such as *.js and *.css
            app.UseStaticFiles();

            //Any client server communication other than static files, 
            //must be inspected by authentication middleware
            app.UseAuthentication();

            //Add MVC into middleware pipeline
            app.UseMvcWithDefaultRoute();
        }
    }
}
