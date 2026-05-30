using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MuntersInterview.Giphy.Manager.Contract;

namespace MuntersInterview.Giphy.Manager;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGiphyManager(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GiphyManagerOptions>(o =>
        {
            if (int.TryParse(configuration["GIPHY_CACHE_TTL_SECONDS"], out var seconds))
                o.CacheTtl = TimeSpan.FromSeconds(seconds);
        });

        services.AddSingleton<IGiphyManager, GiphyManager>();

        return services;
    }
}
