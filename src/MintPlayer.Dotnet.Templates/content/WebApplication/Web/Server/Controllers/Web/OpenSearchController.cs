using Microsoft.AspNetCore.Mvc;
using MintPlayer.AspNetCore.OpenSearch;

namespace MintPlayer.Dotnet.WebApplication.Web.Server.Controllers.Web;

[Route("[controller]")]
public class OpenSearchController : Controller
{
    private readonly IOpenSearchService _openSearchService;

    public OpenSearchController(IOpenSearchService openSearchService)
    {
        _openSearchService = openSearchService;
    }

    [HttpGet("descriptor.xml")]
    public async Task<IActionResult> Descriptor()
    {
        var descriptor = await _openSearchService.GetDescriptorAsync();
        return Content(descriptor, "application/opensearchdescription+xml");
    }
}
