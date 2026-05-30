using MuntersInterview.Giphy.Accessor.Contract.Models;

namespace MuntersInterview.Giphy.Cache.Contract;

public interface IGiphyCacheAccessor
{
    Task<IReadOnlyList<GifItem>?> SearchAsync(string term, CancellationToken ct = default);
    Task SaveAsync(string term, IReadOnlyList<GifItem> items, TimeSpan ttl, CancellationToken ct = default);
    Task<IReadOnlyList<GifItem>?> GetTrendingAsync(CancellationToken ct = default);
    Task SaveTrendingAsync(IReadOnlyList<GifItem> items, TimeSpan ttl, CancellationToken ct = default);
}
