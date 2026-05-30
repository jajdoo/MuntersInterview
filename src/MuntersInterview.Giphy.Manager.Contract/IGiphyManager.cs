using CSharpFunctionalExtensions;
using MuntersInterview.Giphy.Accessor.Contract.Models;
using MuntersInterview.Giphy.Manager.Contract.Models;

namespace MuntersInterview.Giphy.Manager.Contract;

public interface IGiphyManager
{
    Task<Result<IReadOnlyList<GifItem>, GiphyManagerError>> SearchAsync(string term, CancellationToken ct = default);
}
