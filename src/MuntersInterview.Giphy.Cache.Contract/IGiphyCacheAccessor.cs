using CSharpFunctionalExtensions;
using MuntersInterview.Giphy.Accessor.Contract;
using MuntersInterview.Giphy.Accessor.Contract.Models;
using MuntersInterview.Giphy.Cache.Contract.Models;

namespace MuntersInterview.Giphy.Cache.Contract;

public interface IGiphyCacheAccessor : IGiphyAccessor
{
    Task<UnitResult<GiphyCacheError>> SaveAsync(string term, IReadOnlyList<GifItem> items, TimeSpan ttl, CancellationToken ct = default);
}
