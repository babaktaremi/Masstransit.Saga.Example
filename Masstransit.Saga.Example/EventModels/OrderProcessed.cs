namespace Masstransit.Saga.Example.EventModels;

public class OrderProcessed
{
    public Guid OrderId { get; set; }
    public string OrderName { get; set; }
    public string CustomerName { get; set; }
}