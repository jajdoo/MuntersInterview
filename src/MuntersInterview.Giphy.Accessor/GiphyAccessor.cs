using Microsoft.Extensions.Options;
using MuntersInterview.Giphy.Accessor.Contract;
using MuntersInterview.Giphy.Accessor.Contract.Models;
using MuntersInterview.Giphy.Accessor.Models;
using System.Net;
using System.Net.Http.Json;

namespace MuntersInterview.Giphy.Accessor;

internal sealed class GiphyAccessor(HttpClient httpClient, IOptions<GiphyAccessorOptions> options) : IGiphyAccessor
{
    private readonly GiphyAccessorOptions _options = options.Value;

    public async Task<IReadOnlyList<GifItem>> SearchAsync(string term, CancellationToken ct = default)
    {
        var url = $"/v1/gifs/search?api_key={_options.ApiKey}&q={Uri.EscapeDataString(term)}&limit={_options.Limit}";
        var response = await httpClient.GetFromJsonAsync<GiphySearchResponse>(url, ct);
        return response?.Data
            .Select(g => new GifItem { Url = g.Images.Original.Url })
            .ToList() ?? [];
    }

    public async Task<IReadOnlyList<GifItem>> GetTrendingAsync(CancellationToken ct = default)
    {
        var url = $"/v1/gifs/trending?api_key={_options.ApiKey}&limit={_options.Limit}";
        var response = await httpClient.GetFromJsonAsync<GiphySearchResponse>(url, ct);
        return response?.Data
            .Select(g => new GifItem { Url = g.Images.Original.Url })
            .ToList() ?? [];
    }
}
