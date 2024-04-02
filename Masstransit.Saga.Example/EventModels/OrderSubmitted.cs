namespace Masstransit.Saga.Example.EventModels;

public class OrderSubmitted
{
    public Guid OrderId { get; set; }
    public string OrderName { get; set; }
    public string CustomerName { get; set; }
}