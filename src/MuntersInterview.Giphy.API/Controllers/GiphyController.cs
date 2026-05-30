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

        var result = await manager.SearchAsync(term, ct);

        return result switch
        {
            { IsSuccess: true, Value: var gifs } => Ok(new SearchResponse(gifs)),
            { Error: var error }                 => StatusCode(StatusCodes.Status500InternalServerError, new { error }),
        };
    }

    private record SearchResponse(IReadOnlyList<GifItem> Gifs);
}
