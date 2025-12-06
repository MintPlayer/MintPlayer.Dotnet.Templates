namespace MintPlayer.Dotnet.WebApplication.Dtos.Dtos;

public class LoginResponse
{
    public User? User { get; set; }
    public bool RequiresTwoFactor { get; set; }
    public string? Token { get; set; }
}
