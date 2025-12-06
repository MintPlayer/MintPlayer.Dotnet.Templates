namespace MintPlayer.Dotnet.WebApplication.Dtos.Dtos;

public class TwoFactorSetupResponse
{
    public string SharedKey { get; set; } = string.Empty;
    public string AuthenticatorUri { get; set; } = string.Empty;
}
