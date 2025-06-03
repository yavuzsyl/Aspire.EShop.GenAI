using MassTransit;
using Shared.Bus.Events;

namespace Catalog.Application.UseCases.Commands;
public class ProductCommands(ProductDbContext context, IBus bus)
{
    public async Task CreateAsync(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product currentProduct, Product product)
    {
        var hasPriceChanged = currentProduct.Price != product.Price;

        currentProduct.Price = product.Price;
        currentProduct.Name = product.Name;
        currentProduct.Description = product.Description;
        currentProduct.ImageUrl = product.ImageUrl;

        context.Products.Update(currentProduct);
        await context.SaveChangesAsync();

        if (!hasPriceChanged) return;

        var priceChangedEvent = new ProductPriceChangedEvent
        {
            ProductId = currentProduct.Id,
            Price = product.Price,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            Name = product.Name,
        };

        //TODO: for demo purpose made it simple, implement outbox !
        await bus.Publish(priceChangedEvent);

    }

    public async Task DeleteAsync(Product product)
    {
        context.Remove(product);
        await context.SaveChangesAsync();
    }
}
