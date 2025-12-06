namespace MintPlayer.Dotnet.WebApplication.Data.Options;

public class AppOptions
{
    public string SiteName { get; set; } = "MintPlayer.Dotnet.WebApplication";
    public string BaseUrl { get; set; } = "https://localhost:7000";
#if (EnableEmailConfirmation)
    public EmailOptions Email { get; set; } = new();
#endif
}

#if (EnableEmailConfirmation)
public class EmailOptions
{
    public string SmtpHost { get; set; } = "localhost";
    public int SmtpPort { get; set; } = 25;
    public string FromAddress { get; set; } = "noreply@example.com";
    public string FromName { get; set; } = "MintPlayer.Dotnet.WebApplication";
    public bool UseSsl { get; set; } = false;
    public string? Username { get; set; }
    public string? Password { get; set; }
}
#endif
