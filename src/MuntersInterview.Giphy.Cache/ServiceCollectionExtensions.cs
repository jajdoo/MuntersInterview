using Microsoft.Extensions.DependencyInjection;
using MuntersInterview.Giphy.Cache.Contract;

namespace MuntersInterview.Giphy.Cache;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGiphyCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<IGiphyCacheAccessor, GiphyCacheAccessor>();

        return services;
    }
}
