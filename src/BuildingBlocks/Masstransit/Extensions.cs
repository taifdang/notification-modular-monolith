
using BuildingBlocks.Configuration;
using BuildingBlocks.Contracts;
using BuildingBlocks.Exception;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Masstransit;

//ref: https://www.jbw.codes/blog/mass-transit-consumer
//ref: https://masstransit.io/documentation/configuration/middleware/scoped
//ref: https://stackoverflow.com/questions/17326185/what-are-the-different-kinds-of-cases
//ref: https://markgossa.com/2022/06/masstransit-exponential-back-off.html
//ref: https://masstransit.io/documentation/concepts/exceptions
public static class Extensions
{
    
    public static IServiceCollection AddCustomMasstransit(
        this IServiceCollection services,
        TransportType transportType,
        params Assembly[] assemblies)
    {
        services.AddMassTransit(configure =>
        {
            SetupMasstransitConfigurations(services, configure, transportType, assemblies);
        });

        return services;
    }

    public static IServiceCollection SetupMasstransitConfigurations(
        this IServiceCollection services,
        IBusRegistrationConfigurator configure,
        TransportType transportType,
        params Assembly[] assemblies)
    {    
        configure.SetKebabCaseEndpointNameFormatter();

        configure.AddConsumers(assemblies);

        switch (transportType) 
        {
            case TransportType.RabbitMq:
                configure.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitMqOptions = services.GetOptions<RabbitMqOptions>(nameof(RabbitMqOptions));

                    cfg.Host(
                        rabbitMqOptions?.HostName, 
                        rabbitMqOptions?.Port ?? 5672,
                        "/",
                        o =>
                        {
                            o.Username(rabbitMqOptions?.UserName);
                            o.Password(rabbitMqOptions?.Password);
                        });

                    cfg.Message<NotificationMessage>(x =>
                    {
                        x.SetEntityName("notification-exchange");
                    });

                    cfg.ConfigureEndpoints(context);

                    cfg.UseMessageRetry(AddRetryConfiguration);
                });

                break;
            case TransportType.InMemory:
                configure.UsingInMemory(
                    (context, cfg) =>
                    {
                        cfg.ConfigureEndpoints(context);

                        cfg.UseMessageRetry(AddRetryConfiguration);
                    });

                break;

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(transportType),
                    transportType,
                    message: null);
        }

        return services;

    }

    private static void AddRetryConfiguration(IRetryConfigurator configurator)
    {
        configurator.Exponential(
                3,
                TimeSpan.FromMilliseconds(200),
                TimeSpan.FromMinutes(30),
                TimeSpan.FromMilliseconds(200))
            .Ignore<ValidationException>();
    }
}
