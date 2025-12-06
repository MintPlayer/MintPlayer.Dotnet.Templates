namespace MintPlayer.Dotnet.WebApplication.Dtos.Dtos;

public class SearchResult<T>
{
    public List<T> Items { get; set; } = new();
    public long TotalCount { get; set; }
}
