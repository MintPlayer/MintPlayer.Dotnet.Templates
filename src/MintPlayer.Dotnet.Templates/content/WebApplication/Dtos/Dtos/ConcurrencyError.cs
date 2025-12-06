namespace MintPlayer.Dotnet.WebApplication.Dtos.Dtos;

public class ConcurrencyError
{
    public string Message { get; set; } = "The record has been modified by another user.";
    public string? CurrentValue { get; set; }
    public string? DatabaseValue { get; set; }
}
