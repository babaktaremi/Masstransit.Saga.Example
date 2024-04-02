using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Masstransit.Saga.Example.StateModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Masstransit.Saga.Example.SagaRepositories;

public class OrderSagaRepository:SagaDbContext
{
    public OrderSagaRepository(DbContextOptions options) : base(options)
    {
    }

    protected override IEnumerable<ISagaClassMap> Configurations { get { yield return new OrderStateMap(); } }
}

public class OrderStateMap :
    SagaClassMap<OrderSaga>
{
    protected override void Configure(EntityTypeBuilder<OrderSaga> entity, ModelBuilder model)
    {
        entity.Property(x => x.CurrentState).HasMaxLength(64);
        entity.Property(x => x.OrderId);
        entity.Property(x => x.CustomerName);
        entity.Property(x => x.OrderName);
        entity.Property<byte[]>("RowVersion").IsRowVersion();
    }
}