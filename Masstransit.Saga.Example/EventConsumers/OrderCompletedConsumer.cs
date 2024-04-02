using MassTransit;
using Masstransit.Saga.Example.EventModels;

namespace Masstransit.Saga.Example.EventConsumers;

public class OrderCompletedConsumer(ILogger<OrderCompletedConsumer> logger):IConsumer<OrderCompleted>
{
    public async Task Consume(ConsumeContext<OrderCompleted> context)
    {

        await Task.Delay(4000);

        logger.LogWarning("Order successfully completed  with Id {orderId}", context.Message.OrderId);
        logger.LogWarning("Sending Completed Email...");


    }
}