using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using MuntersInterview.Giphy.Accessor.Contract;
using MuntersInterview.Giphy.Accessor.Contract.Models;
using MuntersInterview.Giphy.Cache.Contract;
using MuntersInterview.Giphy.Manager.Contract;
using System.Collections.Concurrent;

namespace MuntersInterview.Giphy.Manager;

internal sealed class GiphyManager(
    IGiphyAccessor accessor,
    IGiphyCacheAccessor cache,
    IOptions<GiphyManagerOptions> options) : IGiphyManager
{
    private readonly GiphyManagerOptions _options = options.Value;
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

    public async Task<Result<IReadOnlyList<GifItem>>> SearchAsync(string term, CancellationToken ct = default)
    {
        var normalizedTerm = term.Trim().ToLowerInvariant();

        var cacheResult = await cache.SearchAsync(normalizedTerm, ct);
        if (cacheResult.IsSuccess)
            return cacheResult;

        var semaphore = _locks.GetOrAdd(normalizedTerm, _ => new SemaphoreSlim(1, 1));
        await semaphore.WaitAsync(ct);
        try
        {
            // Double-check: another waiter may have already populated the cache
            cacheResult = await cache.SearchAsync(normalizedTerm, ct);
            if (cacheResult.IsSuccess)
                return cacheResult;

            var fetchResult = await accessor.SearchAsync(normalizedTerm, ct);
            if (fetchResult.IsFailure)
                return fetchResult;

            await cache.SaveAsync(normalizedTerm, fetchResult.Value, _options.CacheTtl, ct);
            return fetchResult;
        }
        finally
        {
            semaphore.Release();
        }
    }
}
