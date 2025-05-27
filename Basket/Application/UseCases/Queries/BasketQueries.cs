using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Application.UseCases.Queries;

public class BasketQueries(IDistributedCache cache)
{
    public async Task<ShoppingCart?> Get(string userName)
    {
        var basket = await cache.GetStringAsync(userName);
        return string.IsNullOrEmpty(basket) ? null : 
            JsonSerializer.Deserialize<ShoppingCart>(basket);
    }
}
