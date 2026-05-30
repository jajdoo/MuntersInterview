using Microsoft.Extensions.Caching.Memory;
using MuntersInterview.Giphy.Accessor.Contract.Models;
using MuntersInterview.Giphy.Cache.Contract;

namespace MuntersInterview.Giphy.Cache;

internal sealed class GiphyCacheAccessor(IMemoryCache memoryCache) : IGiphyCacheAccessor
{
    public Task<IReadOnlyList<GifItem>?> SearchAsync(string term, CancellationToken ct = default)
    {
        memoryCache.TryGetValue(CacheKey(term), out IReadOnlyList<GifItem>? items);
        return Task.FromResult(items);
    }

    public Task SaveAsync(string term, IReadOnlyList<GifItem> items, TimeSpan ttl, CancellationToken ct = default)
    {
        var entryOptions = new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = ttl };
        memoryCache.Set(CacheKey(term), items, entryOptions);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<GifItem>?> GetTrendingAsync(CancellationToken ct = default)
    {
        memoryCache.TryGetValue(TrendingCacheKey, out IReadOnlyList<GifItem>? items);
        return Task.FromResult(items);
    }

    public Task SaveTrendingAsync(IReadOnlyList<GifItem> items, TimeSpan ttl, CancellationToken ct = default)
    {
        var entryOptions = new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = ttl };
        memoryCache.Set(TrendingCacheKey, items, entryOptions);
        return Task.CompletedTask;
    }

    private const string TrendingCacheKey = "giphy:trending";
    private static string CacheKey(string term) => $"giphy:search:{term.ToLowerInvariant()}";
}
