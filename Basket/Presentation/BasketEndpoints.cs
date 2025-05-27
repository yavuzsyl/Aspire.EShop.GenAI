namespace Basket.Presentation;

public static class BasketEndpoints
{
    public static void MapBasketEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("basket");

        group.MapGet("/{userName}", async (string userName, BasketQueries query) =>
        {
            var basket = await query.Get(userName);
            if (basket == null) return Results.NotFound();

            return Results.Ok(basket);
        })
        .WithName("Get")
        .Produces<ShoppingCart>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (ShoppingCart cart, BasketCommands command) =>
        {

            await command.Update(cart);
            return Results.Created("GetBasket", cart);
        })
        .WithName("Update")
        .Produces<ShoppingCart>(StatusCodes.Status201Created);


        group.MapDelete("/{userName}", async (string userName, BasketCommands command) =>
        {
            await command.Delete(userName);
            return Results.Ok();
        })
        .WithName("Delete")
        .Produces(StatusCodes.Status204NoContent);
    }
}
