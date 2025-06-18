namespace Catalog.Application.UseCases.Queries;

public class ProductQueries(ProductDbContext context)
{
    public async Task<IEnumerable<Product>> GetAsync() =>
        await context.Products.ToListAsync();
    public async Task<Product?> GetByIdAsync(int id) =>
         await context.Products.FindAsync(id);
    public async Task<List<Product>?> GetByIdsAsync(List<int> ids) =>
     await context.Products.Where(p => ids.Contains(p.Id)).ToListAsync();

    public async Task<List<Product>> SearchProductsAsync(string name)
    {
        return await context.Products
            .Where(p => p.Name.Contains(name) || p.Description.Contains(name))
            .ToListAsync();
    }
}
