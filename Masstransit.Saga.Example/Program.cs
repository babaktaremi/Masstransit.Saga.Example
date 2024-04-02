using MassTransit;
using Masstransit.Saga.Example.EventConsumers;
using Masstransit.Saga.Example.EventModels;
using Masstransit.Saga.Example.SagaRepositories;
using Masstransit.Saga.Example.StateMachines;
using Masstransit.Saga.Example.StateModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq((context, factoryConfigurator) =>
    {
       factoryConfigurator.ConfigureEndpoints(context,endpointNameFormatter:new KebabCaseEndpointNameFormatter(true));
       factoryConfigurator.UseDelayedMessageScheduler();
    });
    configurator.AddConsumer<OrderSubmittedConsumer>();
    configurator.AddConsumer<OrderProcessedConsumer>();
    configurator.AddConsumer<OrderCompletedConsumer>();

    configurator.AddSagaStateMachine<OrderSagaStateMachine, OrderSaga>()
        .EntityFrameworkRepository(repositoryConfigurator =>
        {
            repositoryConfigurator.ConcurrencyMode = ConcurrencyMode.Optimistic;
            repositoryConfigurator.AddDbContext<DbContext, OrderSagaRepository>((provider, optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(
                    "Data Source=localhost;Initial Catalog=MasstransitSagaTests;Integrated Security=true;Encrypt=False");
            });
        });
});

var app = builder.Build();

app.MapPost("SubmitOrder", async (AddOrder model, IBus bus) =>
{
    await bus.Publish(new OrderSubmitted()
    {
        OrderId = model.OrderId,
        OrderName = model.OrderName,
        CustomerName = model.CustomerName
    });

    return Results.Accepted();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();




app.Run();

internal record AddOrder(Guid OrderId,string OrderName,string CustomerName);