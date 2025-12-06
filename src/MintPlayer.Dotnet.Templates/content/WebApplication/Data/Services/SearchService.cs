using MintPlayer.Dotnet.WebApplication.Data.Services.Interfaces;
using MintPlayer.Dotnet.WebApplication.Dtos.Dtos;
using Nest;

namespace MintPlayer.Dotnet.WebApplication.Data.Services;

public class SearchService : ISearchService
{
    private readonly IElasticClient _elasticClient;

    public SearchService(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<SearchResult<T>> SearchAsync<T>(SearchRequest request) where T : class
    {
        var searchResponse = await _elasticClient.SearchAsync<T>(s => s
            .From(request.Skip)
            .Size(request.Take)
            .Query(q => q
                .MultiMatch(m => m
                    .Query(request.Query)
                    .Fuzziness(Fuzziness.Auto)
                )
            )
        );

        return new SearchResult<T>
        {
            Items = searchResponse.Documents.ToList(),
            TotalCount = searchResponse.Total
        };
    }

    public async Task IndexAsync<T>(T document) where T : class
    {
        await _elasticClient.IndexDocumentAsync(document);
    }

    public async Task DeleteAsync<T>(string id) where T : class
    {
        await _elasticClient.DeleteAsync<T>(id);
    }

    public async Task ReindexAllAsync<T>(IEnumerable<T> documents) where T : class
    {
        var indexName = typeof(T).Name.ToLowerInvariant();

        // Delete existing index
        await _elasticClient.Indices.DeleteAsync(indexName);

        // Create new index
        await _elasticClient.Indices.CreateAsync(indexName, c => c
            .Map<T>(m => m.AutoMap())
        );

        // Bulk index documents
        if (documents.Any())
        {
            await _elasticClient.BulkAsync(b => b
                .Index(indexName)
                .IndexMany(documents)
            );
        }
    }
}
