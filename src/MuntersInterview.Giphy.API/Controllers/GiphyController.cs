using Microsoft.AspNetCore.Mvc;
using MuntersInterview.Giphy.Accessor.Contract.Models;
using MuntersInterview.Giphy.Manager.Contract;

namespace MuntersInterview.Giphy.API.Controllers;

[ApiController]
[Route("api/giphy")]
public class GiphyController(IGiphyManager manager) : ControllerBase
{
    [HttpGet("search")]
    public async Task<IActionResult> SearchAsync([FromQuery] string? term, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(term))
            return BadRequest(new { error = "term is required" });

        var gifs = await manager.SearchAsync(term, ct);
        return Ok(new SearchResponse(gifs));
    }

    [HttpGet("trending")]
    public async Task<IActionResult> GetTrendingAsync(CancellationToken ct)
    {
        var gifs = await manager.GetTrendingAsync(ct);
        return Ok(new TrendingResponse(gifs));
    }

    private record SearchResponse(IReadOnlyList<GifItem> Gifs);
    private record TrendingResponse(IReadOnlyList<GifItem> Gifs);
}
