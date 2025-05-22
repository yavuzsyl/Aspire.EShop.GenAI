namespace Catalog.Infrastructure.Persistence;
public static class Extensions
{
    public static void UseMigration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

        context.Database.Migrate();
        DataSeeder.Seed(context);
    }
}

public static class DataSeeder
{
    public static void Seed(ProductDbContext context)
    {
        if (context.Products.Any())
            return;

        context.Products.AddRange(Products);
        context.SaveChanges();
    }

    public static IEnumerable<Product> Products =>
        [
            new Product()
            {
                Id = 1,
                Name = "Wireless Mouse",
                Description = "Ergonomic wireless mouse with adjustable DPI.",
                Price = 25.99m,
                ImageUrl = "https://example.com/images/mouse.jpg"
            },
            new Product()
            {
                Id = 2,
                Name = "Mechanical Keyboard",
                Description = "RGB backlit mechanical keyboard with brown switches.",
                Price = 79.99m,
                ImageUrl = "https://example.com/images/keyboard.jpg"
            },
            new Product()
            {
                Id = 3,
                Name = "USB-C Hub",
                Description = "7-in-1 USB-C hub with HDMI, USB 3.0, and SD card reader.",
                Price = 39.50m,
                ImageUrl = "https://example.com/images/usbc-hub.jpg"
            },
            new Product()
            {
                Id = 4,
                Name = "Noise-Cancelling Headphones",
                Description = "Over-ear headphones with active noise cancellation and 30-hour battery life.",
                Price = 129.00m,
                ImageUrl = "https://example.com/images/headphones.jpg"
            },
            new Product()
            {
                Id = 5,
                Name = "Portable SSD 1TB",
                Description = "High-speed 1TB portable solid-state drive.",
                Price = 99.99m,
                ImageUrl = "https://example.com/images/ssd.jpg"
            },
            new Product()
            {
                Id = 6,
                Name = "Webcam 1080p",
                Description = "Full HD 1080p webcam with built-in microphone for clear video calls.",
                Price = 45.00m,
                ImageUrl = "https://example.com/images/webcam.jpg"
            },
            new Product()
            {
                Id = 7,
                Name = "Monitor Stand",
                Description = "Adjustable monitor stand with desk organizer.",
                Price = 19.95m,
                ImageUrl = "https://example.com/images/monitor-stand.jpg"
            },
            new Product()
            {
                Id = 8,
                Name = "Gaming Mouse Pad",
                Description = "Large gaming mouse pad with non-slip base and stitched edges.",
                Price = 14.25m,
                ImageUrl = "https://example.com/images/mousepad.jpg"
            },
            new Product()
            {
                Id = 9,
                Name = "Smart Light Bulb",
                Description = "Wi-Fi enabled smart light bulb with millions of colors and voice control.",
                Price = 12.99m,
                ImageUrl = "https://example.com/images/lightbulb.jpg"
            },
            new Product()
            {
                Id = 10,
                Name = "Travel Backpack",
                Description = "Water-resistant travel backpack with laptop compartment and multiple pockets.",
                Price = 55.00m,
                ImageUrl = "https://example.com/images/backpack.jpg"
            }
        ];

}