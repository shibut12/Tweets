using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Tweets.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //Configure MVC in services collection
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Allow serving static files such as *.js and *.css
            app.UseStaticFiles();

            //Add MVC into middleware pipeline
            app.UseMvcWithDefaultRoute();
        }
    }
}
