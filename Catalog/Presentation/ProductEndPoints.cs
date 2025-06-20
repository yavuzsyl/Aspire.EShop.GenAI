﻿namespace Catalog.Presentation;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products");

        group.MapGet("/", async (ProductQueries query) =>
        {
            var products = await query.GetAsync();
            return Results.Ok(products);
        })
        .WithName("GetAll")
        .Produces<List<Product>>(StatusCodes.Status200OK);

        group.MapPost("/list", async (GetProductsByIdsRequest request, ProductQueries query) =>
        {
            var products = await query.GetByIdsAsync(request.ids);
            return Results.Ok(products);
        })
        .WithName("GetAllByIds")
        .Produces<List<Product>>(StatusCodes.Status200OK);

        group.MapGet("/{id}", async (int id, ProductQueries query) =>
        {
            var product = await query.GetByIdAsync(id);
            return Results.Ok(product);
        })
        .WithName("Get")
        .Produces<Product>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (Product product, ProductCommands command) =>
        {
            await command.CreateAsync(product);
            return Results.Created($"/products/{product.Id}", product);
        })
        .WithName("Create")
        .Produces<Product>(StatusCodes.Status201Created);

        group.MapPut("/{id}",
            async (int id,
            Product product, 
            ProductCommands command, 
            ProductQueries query) =>
        {
            var currentProduct = await query.GetByIdAsync(id);
            if (currentProduct is null) return Results.NotFound();

            await command.UpdateAsync(currentProduct, product);
            return Results.Created($"/products/{product.Id}", product);
        })
        .WithName("Update")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id}",
            async (int id,
            ProductCommands command,
            ProductQueries query) =>
            {
                var product = await query.GetByIdAsync(id);
                if (product is null) return Results.NotFound();

                await command.DeleteAsync(product);
                return Results.NoContent();
            })
        .WithName("Delete")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);


        group.MapGet("/support/{query}", async (string query, ProductAIService service) =>
        {
            var response = await service.SupportAsync(query);
            return Results.Ok(response);
        })
        .WithName("Support")
        .Produces(StatusCodes.Status200OK);

        group.MapGet("/search/{query}", async (string query, ProductQueries queries) =>
        {
            var response = await queries.SearchProductsAsync(query);
            return Results.Ok(response);
        })
        .WithName("Search")
        .Produces<List<Product>>(StatusCodes.Status200OK);

        group.MapGet("/semantic-search/{query}", async (string query, ProductAIService service) =>
        {
            var response = await service.SearchProductsAsync(query);
            return Results.Ok(response);
        })
        .WithName("SearchAI")
        .Produces<List<Product>>(StatusCodes.Status200OK);
    }
}
