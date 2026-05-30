using System.Text.Json.Serialization;

namespace MuntersInterview.Giphy.Accessor.Models;

internal record GiphySearchResponse(
    [property: JsonPropertyName("data")] IReadOnlyList<GiphyGif> Data);

internal record GiphyGif(
    [property: JsonPropertyName("images")] GiphyImages Images);

internal record GiphyImages(
    [property: JsonPropertyName("original")] GiphyImageEntry Original,
    [property: JsonPropertyName("fixed_height")] GiphyImageEntry FixedHeight);

internal record GiphyImageEntry(
    [property: JsonPropertyName("url")] string Url);
