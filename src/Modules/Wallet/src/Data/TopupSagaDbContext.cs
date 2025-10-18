using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet.StateMachines;

namespace Wallet.Data;

public class TopupSagaDbContext : SagaDbContext
{
    public TopupSagaDbContext(DbContextOptions<TopupSagaDbContext> options) : base(options)
    {
    }
    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get
        {
            yield return new TopupStateMap();
        }
    }
}
public class TopupStateMap : SagaClassMap<TopupState>
{
    protected override void Configure(EntityTypeBuilder<TopupState> entity, ModelBuilder model)
    {
        //customize the mapping here
        entity.ToTable("TopupSaga");
        entity.Property(x => x.CurrentState).HasMaxLength(64);
    }
}
