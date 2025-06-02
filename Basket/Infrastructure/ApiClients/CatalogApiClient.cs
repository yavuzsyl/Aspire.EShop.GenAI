using Catalog.Domain;

namespace Basket.Infrastructure.ApiClients;

public class CatalogApiClient(HttpClient httpClient)
{
    public async Task<List<Product?>> GetProductsByIdsAsync(List<int> ids)
    {
        var response = await httpClient.PostAsJsonAsync($"/products/list", new { ids });
        var result = await response.Content.ReadFromJsonAsync<List<Product?>>();
        return result!;
    }
}
