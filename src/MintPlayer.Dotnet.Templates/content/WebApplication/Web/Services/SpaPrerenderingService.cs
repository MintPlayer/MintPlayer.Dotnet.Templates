using MintPlayer.AspNetCore.SpaServices.Prerendering;
using MintPlayer.AspNetCore.SpaServices.Routing;

namespace MintPlayer.Dotnet.WebApplication.Web.Services;

public class SpaPrerenderingService : ISpaPrerenderingService
{
    public Task OnSupplyData(HttpContext context, IDictionary<string, object> data)
    {
        // Add data to be passed to the Angular app during SSR
        data["isServer"] = true;
        data["requestUrl"] = context.Request.Path.ToString();

        return Task.CompletedTask;
    }
}
