using CSharpFunctionalExtensions;
using Microsoft.Extensions.Caching.Memory;
using MuntersInterview.Giphy.Accessor.Contract.Models;
using MuntersInterview.Giphy.Cache.Contract;

namespace MuntersInterview.Giphy.Cache;

internal sealed class GiphyCacheAccessor(IMemoryCache memoryCache) : IGiphyCacheAccessor
{
    public Task<Result<IReadOnlyList<GifItem>>> SearchAsync(string term, CancellationToken ct = default)
    {
        var key = CacheKey(term);

        return memoryCache.TryGetValue(key, out IReadOnlyList<GifItem>? items) && items is not null
            ? Task.FromResult(Result.Success(items))
            : Task.FromResult(Result.Failure<IReadOnlyList<GifItem>>("cache-miss"));
    }

    public Task<Result> SaveAsync(string term, IReadOnlyList<string> items, TimeSpan ttl, CancellationToken ct = default)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl
        };

        memoryCache.Set(CacheKey(term), items, options);

        return Task.FromResult(Result.Success());
    }

    private static string CacheKey(string term) => $"giphy:search:{term.ToLowerInvariant()}";
}
