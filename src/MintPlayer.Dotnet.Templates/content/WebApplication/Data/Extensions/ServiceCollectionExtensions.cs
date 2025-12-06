using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MintPlayer.Dotnet.WebApplication.Data.Persistance;
#if (UseAnyIdentityProvider)
using MintPlayer.Dotnet.WebApplication.Data.Services;
using MintPlayer.Dotnet.WebApplication.Data.Services.Interfaces;
#endif
#if (UseElasticSearch)
using MintPlayer.Dotnet.WebApplication.Data.Services;
using MintPlayer.Dotnet.WebApplication.Data.Services.Interfaces;
#endif

namespace MintPlayer.Dotnet.WebApplication.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure DbContext
        services.AddDbContext<AppDbContext>(options =>
        {
#if (UseSqlServer)
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
#elif (UsePostgreSQL)
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
#elif (UseSQLite)
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
#endif
#if (UseOpenIdDict)
            options.UseOpenIddict();
#endif
        });

#if (UseAnyIdentityProvider)
        // Register account service
        services.AddScoped<IAccountService, AccountService>();
#endif

#if (UseElasticSearch)
        // Register search service
        services.AddScoped<ISearchService, SearchService>();
#endif

        return services;
    }
}
