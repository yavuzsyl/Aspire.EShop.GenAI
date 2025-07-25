﻿using Catalog.Domain;

namespace WebApp.ApiClients;

public class CatalogApiClient(HttpClient httpClient)
{
    public const string Path = "catalog/products";
    public async Task<List<Product>> GetProductsAsync()
    {
        var response = await httpClient.GetFromJsonAsync<List<Product>>(Path);
        return response!;
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        var response = await httpClient.GetFromJsonAsync<Product>($"{Path}/{id}");
        return response!;
    }

    public async Task<string> SupportChat(string query)
    {
        var response = await httpClient.GetFromJsonAsync<string>($"{Path}/support/{query}");
        return response!;
    }

    public async Task<List<Product>?> SearchProducts(string query, bool aiSearch)
    {
        if (aiSearch) return await httpClient.GetFromJsonAsync<List<Product>?>($"{Path}/semantic-search/{query}");

        return await httpClient.GetFromJsonAsync<List<Product>?>($"{Path}/search/{query}");
    }
}
