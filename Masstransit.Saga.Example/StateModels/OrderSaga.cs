using MassTransit;

namespace Masstransit.Saga.Example.StateModels;

public class OrderSaga:SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public Guid OrderId { get; set; }
    public string OrderName { get; set; }
    public string CustomerName { get; set; }
    public string CurrentState { get; set; }
}