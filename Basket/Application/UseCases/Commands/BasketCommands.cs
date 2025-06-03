using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Application.UseCases.Commands;

public class BasketCommands(IDistributedCache cache, CatalogApiClient catalogApiClient, BasketQueries query)
{
    public async Task Update(ShoppingCart cart)
    {
        var products = await catalogApiClient.GetProductsByIdsAsync(cart.Items.Select(i => i.ProductId).ToList());
        foreach (var item in cart.Items)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product is null) continue;

            item.Price = product.Price;
            item.ProductName = product.Name;
        }

        await cache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(cart));
    }
    public async Task Delete(string userName) => await cache.RemoveAsync(userName);

    public async Task UpdateBasketItemProductPrice(int productId, decimal price)
    {
        var basket = await query.Get("user1"); // for demo purpose
        var item = basket!.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null) return;

        item.Price = price;
        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));

    }
}
