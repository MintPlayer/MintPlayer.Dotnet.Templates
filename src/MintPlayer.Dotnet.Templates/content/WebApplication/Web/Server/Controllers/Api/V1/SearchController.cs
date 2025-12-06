using Microsoft.AspNetCore.Mvc;
using MintPlayer.Dotnet.WebApplication.Data.Services.Interfaces;
using MintPlayer.Dotnet.WebApplication.Dtos.Dtos;

namespace MintPlayer.Dotnet.WebApplication.Web.Server.Controllers.Api.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet]
    public async Task<ActionResult<SearchResult<object>>> Search([FromQuery] SearchRequest request)
    {
        var result = await _searchService.SearchAsync<object>(request);
        return Ok(result);
    }
}
