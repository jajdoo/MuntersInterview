using CSharpFunctionalExtensions;
using MuntersInterview.Giphy.Accessor.Contract.Models;

namespace MuntersInterview.Giphy.Accessor.Contract;

public interface IGiphyAccessor
{
    Task<Result<IReadOnlyList<GifItem>, GiphyAccessError>> SearchAsync(string term, CancellationToken ct = default);
}
