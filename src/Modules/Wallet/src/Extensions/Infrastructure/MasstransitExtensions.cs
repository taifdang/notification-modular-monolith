using BuildingBlocks.Configuration;
using BuildingBlocks.EFCore;
using BuildingBlocks.Masstransit;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Wallet.Data;
using Wallet.Identity.Consumers.RegisteringNewUser;
using Wallet.StateMachines;

namespace Wallet.Extensions.Infrastructure;

public class MasstransitExtensions : IMasstransitModule, IStateMachineModule
{
    public void ConfigureStateMachines(IBusRegistrationConfigurator cfg)
    {
        cfg.AddSagaStateMachine<TopupStateMachine, TopupState, TopupStateMachineDefinition>()
          //.InMemoryRepository();
          .InMemoryRepository()
          .EntityFrameworkRepository(r =>
          {
              //RowVersion 
              //r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
              r.AddDbContext<DbContext, TopupSagaDbContext>((provider, builder) =>
              {
                  //var options = provider.GetOptions<MssqlOptions>("mssql").ConnectionString;
                  builder.UseSqlServer("Server=localhost,1433;Database=notification_db;User Id=sa;Password=Password!;Trust Server Certificate=True");
              });
          });
    }

    public void ConfigureTopology(IBusFactoryConfigurator cfg, IRegistrationContext context)
    {
        cfg.ReceiveEndpoint("identity-wallet", e =>
        {          
            e.ConfigureConsumer<RegisterNewUserHandler>(context);
        });

    }
}

//public static class NotificationModuleMassTransitConfig
//{
//    public static IServiceCollection AddNotificationModule(this IServiceCollection services)
//    {

//        services.AddMassTransit(x =>
//        {
//            x.AddSagaStateMachine<TopupStateMachine, TopupState, TopupStateMachineDefinition>()
//                .InMemoryRepository()
//                .EntityFrameworkRepository(r =>
//                {
//                    //r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
//                    r.AddDbContext<DbContext, WalletDbContext>();
//                });
//        });

//        return services;
//    }
//}
