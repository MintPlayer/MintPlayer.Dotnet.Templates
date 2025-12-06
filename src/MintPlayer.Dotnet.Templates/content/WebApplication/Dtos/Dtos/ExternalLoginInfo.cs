namespace MintPlayer.Dotnet.WebApplication.Dtos.Dtos;

public class ExternalLoginInfo
{
    public string LoginProvider { get; set; } = string.Empty;
    public string ProviderKey { get; set; } = string.Empty;
    public string? ProviderDisplayName { get; set; }
}
