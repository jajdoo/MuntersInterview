using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MuntersInterview.Giphy.Accessor.Contract;

namespace MuntersInterview.Giphy.Accessor;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGiphyAccessor(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GiphyAccessorOptions>(o =>
        {
            o.ApiKey = configuration["GIPHY_API_KEY"] ?? string.Empty;
            o.BaseUrl = configuration["GIPHY_BASE_URL"] ?? "https://api.giphy.com";

            if (int.TryParse(configuration["GIPHY_LIMIT"], out var limit))
                o.Limit = limit;
        });

        services
            .AddHttpClient<IGiphyAccessor, GiphyAccessor>((sp, client) =>
            {
                var opts = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<GiphyAccessorOptions>>().Value;
                client.BaseAddress = new Uri(opts.BaseUrl);
            });

        return services;
    }
}
