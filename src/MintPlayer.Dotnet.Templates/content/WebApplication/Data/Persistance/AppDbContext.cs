#if (UseAnyIdentityProvider)
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
#endif
using Microsoft.EntityFrameworkCore;
#if (UseAnyIdentityProvider)
using MintPlayer.Dotnet.WebApplication.Data.Entities;
#endif

namespace MintPlayer.Dotnet.WebApplication.Data.Persistance;

#if (UseAnyIdentityProvider)
public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
#else
public class AppDbContext : DbContext
#endif
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

#if (UseAnyIdentityProvider)
        // Configure Identity tables
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
#if (EnableConcurrencyHandling)
            entity.Property(e => e.ConcurrencyStamp).IsConcurrencyToken();
#endif
        });

        modelBuilder.Entity<IdentityRole<Guid>>(entity =>
        {
            entity.ToTable("Roles");
        });

        modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
        {
            entity.ToTable("UserRoles");
        });

        modelBuilder.Entity<IdentityUserClaim<Guid>>(entity =>
        {
            entity.ToTable("UserClaims");
        });

        modelBuilder.Entity<IdentityUserLogin<Guid>>(entity =>
        {
            entity.ToTable("UserLogins");
        });

        modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity =>
        {
            entity.ToTable("RoleClaims");
        });

        modelBuilder.Entity<IdentityUserToken<Guid>>(entity =>
        {
            entity.ToTable("UserTokens");
        });
#endif

#if (UseOpenIdDict)
        // Configure OpenIddict tables
        modelBuilder.UseOpenIddict();
#endif
    }
}
