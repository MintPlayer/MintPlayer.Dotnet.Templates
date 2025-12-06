using System.Text.RegularExpressions;
using MintPlayer.AspNetCore.SpaServices.Extensions;
using MintPlayer.AspNetCore.SpaServices.Routing;
#if (UseSsr)
using MintPlayer.AspNetCore.SpaServices.Prerendering;
#endif
#if (UseWebMarkupMin)
using WebMarkupMin.AspNetCore8;
#endif
#if (EnableXsrf)
using MintPlayer.AspNetCore.SpaServices.Xsrf;
#endif
#if (EnableOpenSearch)
using MintPlayer.AspNetCore.OpenSearch;
#endif
#if (UseAnyIdentityProvider)
using Microsoft.AspNetCore.Identity;
using MintPlayer.Dotnet.WebApplication.Data.Entities;
#endif
using MintPlayer.Dotnet.WebApplication.Data.Persistance;
using MintPlayer.Dotnet.WebApplication.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Add Data layer services
builder.Services.AddAppServices(builder.Configuration);

#if (UseAnyIdentityProvider)
// Configure Identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
#if (EnableEmailConfirmation)
    options.SignIn.RequireConfirmedEmail = true;
#else
    options.SignIn.RequireConfirmedEmail = false;
#endif
#if (EnableTwoFactor)
    options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
#endif
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();
#endif

#if (UseIdentityServer)
// Configure IdentityServer
builder.Services.AddIdentityServer()
    .AddApiAuthorization<User, AppDbContext>();
#endif

#if (UseOpenIdDict)
// Configure OpenIddict
builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
            .UseDbContext<AppDbContext>();
    })
    .AddServer(options =>
    {
        options.SetAuthorizationEndpointUris("/connect/authorize")
            .SetTokenEndpointUris("/connect/token")
            .SetUserInfoEndpointUris("/connect/userinfo");

        options.AllowAuthorizationCodeFlow()
            .AllowRefreshTokenFlow();

        options.AddDevelopmentEncryptionCertificate()
            .AddDevelopmentSigningCertificate();

        options.UseAspNetCore()
            .EnableAuthorizationEndpointPassthrough()
            .EnableTokenEndpointPassthrough()
            .EnableUserInfoEndpointPassthrough();
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });
#endif

#if (UseAnyIdentityProvider)
builder.Services.AddAuthentication()
#if (ExternalLoginMicrosoft)
    .AddMicrosoftAccount(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"] ?? "";
        options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"] ?? "";
    })
#endif
#if (ExternalLoginGoogle)
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? "";
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "";
    })
#endif
#if (ExternalLoginFacebook)
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration["Authentication:Facebook:AppId"] ?? "";
        options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"] ?? "";
    })
#endif
#if (ExternalLoginX)
    .AddTwitter(options =>
    {
        options.ConsumerKey = builder.Configuration["Authentication:Twitter:ConsumerKey"] ?? "";
        options.ConsumerSecret = builder.Configuration["Authentication:Twitter:ConsumerSecret"] ?? "";
    })
#endif
#if (ExternalLoginLinkedIn)
    .AddLinkedIn(options =>
    {
        options.ClientId = builder.Configuration["Authentication:LinkedIn:ClientId"] ?? "";
        options.ClientSecret = builder.Configuration["Authentication:LinkedIn:ClientSecret"] ?? "";
    })
#endif
    ;

builder.Services.AddAuthorization();
#endif

#if (EnableXsrf)
// Configure XSRF protection
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.Name = "XSRF-TOKEN";
    options.Cookie.HttpOnly = false;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});
#endif

#if (UseWebMarkupMin)
// Configure HTML minification
builder.Services.AddWebMarkupMin(options =>
{
    options.AllowMinificationInDevelopmentEnvironment = false;
    options.AllowCompressionInDevelopmentEnvironment = false;
})
.AddHtmlMinification(options =>
{
    options.MinificationSettings.RemoveRedundantAttributes = true;
    options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
    options.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;
})
.AddHttpCompression();
#endif

#if (UseElasticSearch)
// Configure ElasticSearch
if (builder.Configuration.GetValue<bool>("ElasticSearch:Active"))
{
    builder.Services.AddSingleton<Nest.IElasticClient>(sp =>
    {
        var settings = new Nest.ConnectionSettings(new Uri(builder.Configuration["ElasticSearch:Url"] ?? "http://localhost:9200"))
            .DefaultIndex(builder.Configuration["ElasticSearch:Index"] ?? "default");
        return new Nest.ElasticClient(settings);
    });
}
#endif

#if (EnableOpenSearch)
// Configure OpenSearch descriptor
builder.Services.AddOpenSearch<MintPlayer.Dotnet.WebApplication.Web.Services.OpenSearchService>();
#endif

// Configure SPA static files
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/dist/client-app/browser";
});

#if (UseSsr)
// Configure SPA prerendering
builder.Services.AddSpaPrerenderingService<MintPlayer.Dotnet.WebApplication.Web.Services.SpaPrerenderingService>();
#endif

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
#if (EnableHsts)
    app.UseHsts();
#endif
}

#if (EnableHttps)
app.UseHttpsRedirection();
#endif

app.UseStaticFiles();
if (!app.Environment.IsDevelopment())
{
    app.UseSpaStaticFilesImproved();
}

#if (UseWebMarkupMin)
app.UseWebMarkupMin();
#endif

app.UseRouting();

#if (UseAnyIdentityProvider)
app.UseAuthentication();
app.UseAuthorization();
#endif

#if (EnableXsrf)
app.UseXsrfProtection();
#endif

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.UseSpaImproved(spa =>
{
    spa.Options.SourcePath = "ClientApp";
#if (UseSsr)
    spa.Options.StartupTimeout = TimeSpan.FromMinutes(5);
#endif

    if (app.Environment.IsDevelopment())
    {
        spa.UseProxyToSpaDevelopmentServer("https://localhost:7000");
    }
});

app.Run();
