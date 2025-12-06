using MintPlayer.Dotnet.WebApplication.Dtos.Dtos;

namespace MintPlayer.Dotnet.WebApplication.Data.Services.Interfaces;

public interface IAccountService
{
    Task<User> RegisterAsync(RegisterRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task LogoutAsync();
    Task<User?> GetCurrentUserAsync();
    Task<User> UpdateProfileAsync(User user);
    Task<bool> ChangePasswordAsync(string currentPassword, string newPassword);
#if (EnableEmailConfirmation)
    Task SendConfirmationEmailAsync(string email);
    Task<bool> ConfirmEmailAsync(string userId, string token);
#endif
#if (EnableTwoFactor)
    Task<TwoFactorSetupResponse> SetupTwoFactorAsync();
    Task<bool> VerifyTwoFactorAsync(string code);
    Task<bool> DisableTwoFactorAsync();
#endif
#if (EnableExternalLogins)
    Task<IEnumerable<ExternalLoginInfo>> GetExternalLoginsAsync();
    Task RemoveExternalLoginAsync(string provider, string providerKey);
#endif
}
