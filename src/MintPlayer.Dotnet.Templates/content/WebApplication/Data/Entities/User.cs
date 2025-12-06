using Microsoft.AspNetCore.Identity;

namespace MintPlayer.Dotnet.WebApplication.Data.Entities;

public class User : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? PictureUrl { get; set; }

#if (EnableConcurrencyHandling)
    public byte[]? RowVersion { get; set; }
#endif
}
