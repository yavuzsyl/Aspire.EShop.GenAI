using Catalog.Domain;

namespace WebApp.ApiClients;

public class CatalogApiClient(HttpClient httpClient)
{
    public async Task<List<Product>> GetProductsAsync()
    {
        var response = await httpClient.GetFromJsonAsync<List<Product>>($"/products");
        return response!;
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        var response = await httpClient.GetFromJsonAsync<Product>($"/products/{id}");
        return response!;
    }

    public async Task<string> SupportChat(string query)
    {
        var response = await httpClient.GetFromJsonAsync<string>($"/products/support/{query}");
        return response!;
    }

    public async Task<List<Product>?> SearchProducts(string query, bool aiSearch)
    {
        if (aiSearch) return await httpClient.GetFromJsonAsync<List<Product>?>($"/products/semantic-search/{query}");

        return await httpClient.GetFromJsonAsync<List<Product>?>($"/products/search/{query}");
    }
}
