using Microsoft.Extensions.DependencyInjection;
using Tweets.Web.Models;

namespace Tweets.Web.Services
{
    public static class TwitterServiceExtensions
    {
        public static IServiceCollection AddTwitterServices(this IServiceCollection services, TwitterServiceOptions options)
        {
            return services.AddTransient<ITwitterService, TwitterService>(_ =>
              new TwitterService(options, services.BuildServiceProvider().GetService<IHttpHelper>()));
        }
    }
}
