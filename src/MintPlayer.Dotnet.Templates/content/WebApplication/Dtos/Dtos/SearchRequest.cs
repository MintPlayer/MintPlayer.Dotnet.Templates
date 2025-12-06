namespace MintPlayer.Dotnet.WebApplication.Dtos.Dtos;

public class SearchRequest
{
    public string Query { get; set; } = string.Empty;
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
}
