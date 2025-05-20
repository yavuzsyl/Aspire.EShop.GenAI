namespace Catalog.Infrastructure.Persistence;
public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options) {
    public DbSet<Product> Products => Set<Product>();
}
