namespace Catalog.Application.UseCases.Queries;

public class ProductQueries(ProductDbContext context)
{
    public async Task<IEnumerable<Product>> GetAsync() => 
        await context.Products.ToListAsync();
    public async Task<Product?> GetByIdAsync(int id) =>
         await context.Products.FindAsync(id);
}
