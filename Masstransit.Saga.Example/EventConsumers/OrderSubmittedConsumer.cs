using Masstransit.Saga.Example.EventModels;
using MassTransit;

namespace Masstransit.Saga.Example.EventConsumers;

public class OrderSubmittedConsumer(ILogger<OrderSubmittedConsumer> logger):IConsumer<OrderSubmitted>
{


    public async Task Consume(ConsumeContext<OrderSubmitted> context)
    {
        await Task.Delay(4000);


        logger.LogWarning("Order submitted by customer {customerName} with Id {orderId} with name {orderName}",context.Message.CustomerName,context.Message.OrderId,context.Message.OrderName);
        logger.LogWarning("Sending Submitted Email...");

    }
}