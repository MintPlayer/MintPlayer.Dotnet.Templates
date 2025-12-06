using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MintPlayer.Dotnet.WebApplication.Data.Services.Interfaces;
using MintPlayer.Dotnet.WebApplication.Dtos.Dtos;

namespace MintPlayer.Dotnet.WebApplication.Web.Server.Controllers.Api.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var user = await _accountService.RegisterAsync(request);
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var response = await _accountService.LoginAsync(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _accountService.LogoutAsync();
        return Ok();
    }

    [HttpGet("current")]
    [Authorize]
    public async Task<ActionResult<User>> GetCurrentUser()
    {
        var user = await _accountService.GetCurrentUserAsync();
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPut("profile")]
    [Authorize]
    public async Task<ActionResult<User>> UpdateProfile([FromBody] User user)
    {
        try
        {
            var updatedUser = await _accountService.UpdateProfileAsync(user);
            return Ok(updatedUser);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var result = await _accountService.ChangePasswordAsync(request.CurrentPassword, request.NewPassword);
        if (!result)
        {
            return BadRequest(new { message = "Failed to change password" });
        }
        return Ok();
    }

#if (EnableTwoFactor)
    [HttpGet("two-factor/setup")]
    [Authorize]
    public async Task<ActionResult<TwoFactorSetupResponse>> SetupTwoFactor()
    {
        var response = await _accountService.SetupTwoFactorAsync();
        return Ok(response);
    }

    [HttpPost("two-factor/verify")]
    [Authorize]
    public async Task<IActionResult> VerifyTwoFactor([FromBody] TwoFactorVerifyRequest request)
    {
        var result = await _accountService.VerifyTwoFactorAsync(request.Code);
        if (!result)
        {
            return BadRequest(new { message = "Invalid verification code" });
        }
        return Ok();
    }

    [HttpPost("two-factor/disable")]
    [Authorize]
    public async Task<IActionResult> DisableTwoFactor()
    {
        var result = await _accountService.DisableTwoFactorAsync();
        if (!result)
        {
            return BadRequest(new { message = "Failed to disable two-factor authentication" });
        }
        return Ok();
    }
#endif

#if (EnableExternalLogins)
    [HttpGet("external-logins")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ExternalLoginInfo>>> GetExternalLogins()
    {
        var logins = await _accountService.GetExternalLoginsAsync();
        return Ok(logins);
    }

    [HttpDelete("external-logins/{provider}/{providerKey}")]
    [Authorize]
    public async Task<IActionResult> RemoveExternalLogin(string provider, string providerKey)
    {
        await _accountService.RemoveExternalLoginAsync(provider, providerKey);
        return Ok();
    }
#endif
}

public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
