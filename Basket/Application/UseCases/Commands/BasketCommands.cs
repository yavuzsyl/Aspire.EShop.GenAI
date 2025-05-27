using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Application.UseCases.Commands;

public class BasketCommands(IDistributedCache cache)
{
    public async Task Update(ShoppingCart cart) => await cache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(cart));
    public async Task Delete(string userName) => await cache.RemoveAsync(userName);
}
