namespace Catalog.Application.UseCases.Commands;
public class ProductCommands(ProductDbContext context)
{
    public async Task CreateAsync(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product currentProduct, Product product)
    {
        currentProduct.Name = product.Name;
        currentProduct.Description = product.Description;
        currentProduct.Price = product.Price;
        currentProduct.ImageUrl = product.ImageUrl;

        context.Products.Update(currentProduct);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        context.Remove(product);
        await context.SaveChangesAsync();
    }
}
