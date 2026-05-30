using MuntersInterview.Giphy.Accessor.Contract.Models;

namespace MuntersInterview.Giphy.Manager.Contract;

public interface IGiphyManager
{
    Task<IReadOnlyList<GifItem>> SearchAsync(string term, CancellationToken ct = default);
    Task<IReadOnlyList<GifItem>> GetTrendingAsync(CancellationToken ct = default);
}
