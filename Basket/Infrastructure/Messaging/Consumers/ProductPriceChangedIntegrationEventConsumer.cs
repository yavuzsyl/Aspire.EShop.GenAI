using MassTransit;
using Shared.Bus.Events;

namespace Basket.Infrastructure.Messaging.Consumers;

public class ProductPriceChangedIntegrationEventConsumer(BasketCommands command): IConsumer<ProductPriceChangedEvent>
{
    public async Task Consume(ConsumeContext<ProductPriceChangedEvent> context)
    {
        await command.UpdateBasketItemProductPrice(context.Message.ProductId, context.Message.Price);
    }
}
