using MassTransit;
using Masstransit.Saga.Example.EventModels;

namespace Masstransit.Saga.Example.EventConsumers;

public class OrderProcessedConsumer(ILogger<OrderProcessedConsumer> logger):IConsumer<OrderProcessed>
{
    public async Task Consume(ConsumeContext<OrderProcessed> context)
    {
        await Task.Delay(4000);


        logger.LogWarning("Order successfully processed by customer {customerName} with Id {orderId} with name {orderName}", context.Message.CustomerName, context.Message.OrderId, context.Message.OrderName);
        logger.LogWarning("Sending Processed Email...");

        await context.Publish(new OrderCompleted()
        {
            OrderId = context.Message.OrderId
        });
    }
}