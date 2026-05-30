using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using MuntersInterview.Giphy.Accessor.Contract;
using MuntersInterview.Giphy.Accessor.Contract.Models;
using MuntersInterview.Giphy.Accessor.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace MuntersInterview.Giphy.Accessor;

internal sealed class GiphyAccessor(HttpClient httpClient, IOptions<GiphyAccessorOptions> options) : IGiphyAccessor
{
    private readonly GiphyAccessorOptions _options = options.Value;

    public async Task<Result<IReadOnlyList<GifItem>, GiphyAccessError>> SearchAsync(string term, CancellationToken ct = default)
    {
        var url = $"/v1/gifs/search?api_key={_options.ApiKey}&q={Uri.EscapeDataString(term)}&limit={_options.Limit}";
        try
        {
            var response = await httpClient.GetFromJsonAsync<GiphySearchResponse>(url, ct);
            var asList = response?.Data
                    .Select(g => new GifItem { Url = g.Images.Original.Url })
                    .ToList();
            return Result.Success<IReadOnlyList<GifItem>, GiphyAccessError>(asList ?? []);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
        }
        catch (OperationCanceledException ex)
        {
        }
        catch (Exception ex)
        {
            return Result.Failure<IReadOnlyList<GifItem>, GiphyAccessError>(new GiphyAccessError.FailedToFetch());
        }
    }

}
