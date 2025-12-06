using MintPlayer.Dotnet.WebApplication.Dtos.Dtos;

namespace MintPlayer.Dotnet.WebApplication.Data.Services.Interfaces;

public interface ISearchService
{
    Task<SearchResult<T>> SearchAsync<T>(SearchRequest request) where T : class;
    Task IndexAsync<T>(T document) where T : class;
    Task DeleteAsync<T>(string id) where T : class;
    Task ReindexAllAsync<T>(IEnumerable<T> documents) where T : class;
}
