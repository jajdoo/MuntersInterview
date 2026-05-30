using MuntersInterview.Giphy.Accessor.Contract.Models;

namespace MuntersInterview.Giphy.Accessor.Contract;

public interface IGiphyAccessor
{
    Task<IReadOnlyList<GifItem>> SearchAsync(string term, CancellationToken ct = default);
    Task<IReadOnlyList<GifItem>> GetTrendingAsync(CancellationToken ct = default);
}
