using MassTransit;
using Masstransit.Saga.Example.EventModels;
using Masstransit.Saga.Example.StateModels;

namespace Masstransit.Saga.Example.StateMachines;

public class OrderSagaStateMachine:MassTransitStateMachine<OrderSaga>
{
    public Event<OrderSubmitted> OrderSubmitted { get;private set; }

    public Event<OrderProcessed> OrderProcessed { get; private set ; }

    public Event<OrderCompleted> OrderCompleted { get; private set; }


    public State Submitted { get; private set; }
    public State Processed { get; private set; }
    public State Completed { get; private set; }

    public OrderSagaStateMachine()
    {
        InstanceState(x=>x.CurrentState);

        Event(() => OrderSubmitted, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => OrderProcessed, x => x.CorrelateById(context => context.Message.OrderId));
        Event(() => OrderCompleted, x => x.CorrelateById(context => context.Message.OrderId));


        Initially(
            When(OrderSubmitted)
                .Then(context =>
                {
                    context.Saga.CustomerName=context.Message.CustomerName;
                    context.Saga.OrderId=context.Message.OrderId;
                    context.Saga.OrderName=context.Message.OrderName;
                }).Publish(context=>new OrderProcessed()
                {
                    OrderName = context.Saga.OrderName,
                    OrderId = context.Saga.OrderId,
                    CustomerName = context.Saga.CustomerName
                })
            .TransitionTo(Processed)
                .Publish(context=>new OrderCompleted()
                {
                    OrderId = context.Saga.OrderId
                })
                .TransitionTo(Completed)
            );

        During(Processed
            ,Ignore(OrderSubmitted));

        During(Completed
            , Ignore(OrderProcessed));

        SetCompletedWhenFinalized();


    }
}