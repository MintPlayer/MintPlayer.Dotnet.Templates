namespace MintPlayer.Dotnet.WebApplication.Dtos.Dtos;

public class User
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
#if (EnableConcurrencyHandling)
    public string? ConcurrencyStamp { get; set; }
#endif
}
