using Microsoft.AspNetCore.Antiforgery;

namespace MintPlayer.Dotnet.WebApplication.Web.Extensions;

public static class XsrfExtensions
{
    public static IApplicationBuilder UseXsrfProtection(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();

            // Generate and set the XSRF token cookie for non-API requests
            if (!context.Request.Path.StartsWithSegments("/api"))
            {
                var tokens = antiforgery.GetAndStoreTokens(context);
                if (tokens.RequestToken != null)
                {
                    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                        new CookieOptions
                        {
                            HttpOnly = false,
                            Secure = context.Request.IsHttps,
                            SameSite = SameSiteMode.Strict
                        });
                }
            }

            await next();
        });
    }
}
