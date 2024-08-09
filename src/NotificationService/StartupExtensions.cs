using MassTransit;
using NotificationService.Consumers;

namespace NotificationService;

public static class StartupExtensions
{
    public static IServiceCollection ConfigureMassTransit(this IServiceCollection services, IConfiguration config)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
            
            x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("nt", false));

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(config["RabbitMq:Host"], "/", host =>
                {
                    host.Username(config.GetValue("RabbitMq:Username", "guest"));
                    host.Password(config.GetValue("RabbitMq:Password", "guest"));
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}