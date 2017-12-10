using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Tweets.Web.Services;

namespace Tweets.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/login";
            })
            .AddTwitter(twitterOptions =>
            {
                twitterOptions.ConsumerKey = "CONSUMER_KEY";
                twitterOptions.ConsumerSecret = "CONSUMER_SECRET";
                twitterOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            //Configure MVC in services collection
            services.AddMvc();

            //Inject TwitterService
            services.AddTransient<ITwitterService, TwitterService>();
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
