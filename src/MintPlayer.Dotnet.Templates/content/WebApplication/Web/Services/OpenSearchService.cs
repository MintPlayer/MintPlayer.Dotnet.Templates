using MintPlayer.AspNetCore.OpenSearch;

namespace MintPlayer.Dotnet.WebApplication.Web.Services;

public class OpenSearchService : IOpenSearchService
{
    private readonly IConfiguration _configuration;

    public OpenSearchService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<string> GetDescriptorAsync()
    {
        var baseUrl = _configuration["App:BaseUrl"] ?? "https://localhost:7000";
        var siteName = _configuration["App:SiteName"] ?? "MintPlayer.Dotnet.WebApplication";

        var xml = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<OpenSearchDescription xmlns=""http://a9.com/-/spec/opensearch/1.1/"">
    <ShortName>{siteName}</ShortName>
    <Description>Search {siteName}</Description>
    <InputEncoding>UTF-8</InputEncoding>
    <Url type=""text/html"" template=""{baseUrl}/search?q={{searchTerms}}""/>
    <Url type=""application/x-suggestions+json"" template=""{baseUrl}/api/v1/search/suggestions?q={{searchTerms}}""/>
</OpenSearchDescription>";

        return Task.FromResult(xml);
    }
}
