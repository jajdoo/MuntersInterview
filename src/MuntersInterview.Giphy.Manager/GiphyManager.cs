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
    private readonly ConcurrentDictionary<string, (SemaphoreSlim Semaphore, int Waiters)> _locks = new();

    public async Task<IReadOnlyList<GifItem>> SearchAsync(string term, CancellationToken ct = default)
    {
        var normalizedTerm = term.Trim().ToLowerInvariant();

        var cached = await cache.SearchAsync(normalizedTerm, ct);
        if (cached is not null)
            return cached;

        var semaphore = AcquireSlot(normalizedTerm);
        await semaphore.WaitAsync(ct);
        try
        {
            // Double-check: another waiter may have already populated the cache
            cached = await cache.SearchAsync(normalizedTerm, ct);
            if (cached is not null)
                return cached;

            var gifs = await accessor.SearchAsync(normalizedTerm, ct);
            await cache.SaveAsync(normalizedTerm, gifs, _options.CacheTtl, ct);
            return gifs;
        }
        finally
        {
            semaphore.Release();
            ReleaseSlot(normalizedTerm);
        }
    }

    public async Task<IReadOnlyList<GifItem>> GetTrendingAsync(CancellationToken ct = default)
    {
        var cached = await cache.GetTrendingAsync(ct);
        if (cached is not null)
            return cached;

        var semaphore = AcquireSlot("trending");
        await semaphore.WaitAsync(ct);
        try
        {
            cached = await cache.GetTrendingAsync(ct);
            if (cached is not null)
                return cached;

            var gifs = await accessor.GetTrendingAsync(ct);
            await cache.SaveTrendingAsync(gifs, _options.CacheTtl, ct);
            return gifs;
        }
        finally
        {
            semaphore.Release();
            ReleaseSlot("trending");
        }
    }

    private SemaphoreSlim AcquireSlot(string key)
    {
        while (true)
        {
            var entry = _locks.AddOrUpdate(
                key,
                _ => (new SemaphoreSlim(1, 1), 1),
                (_, old) => (old.Semaphore, old.Waiters + 1));
            return entry.Semaphore;
        }
    }

    private void ReleaseSlot(string key)
    {
        while (true)
        {
            if (!_locks.TryGetValue(key, out var entry))
                return;

            if (entry.Waiters == 1)
            {
                // Last waiter — try to remove the entry atomically
                if (_locks.TryRemove(new KeyValuePair<string, (SemaphoreSlim, int)>(key, entry)))
                    return;
            }
            else
            {
                var updated = (entry.Semaphore, entry.Waiters - 1);
                if (_locks.TryUpdate(key, updated, entry))
                    return;
            }
            // Lost the race — retry
        }
    }
}
