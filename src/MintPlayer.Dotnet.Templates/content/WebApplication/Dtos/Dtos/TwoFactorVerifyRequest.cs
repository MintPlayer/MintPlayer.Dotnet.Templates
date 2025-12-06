using System.ComponentModel.DataAnnotations;

namespace MintPlayer.Dotnet.WebApplication.Dtos.Dtos;

public class TwoFactorVerifyRequest
{
    [Required]
    [StringLength(6, MinimumLength = 6)]
    public string Code { get; set; } = string.Empty;
}
