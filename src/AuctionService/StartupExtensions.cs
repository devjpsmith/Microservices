using AuctionService.Consumers;
using AuctionService.Data;
using AuctionService.RequestHelpers;
using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AuctionService;

public static class StartupExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        return services.AddDbContext<AuctionDbContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            })
            .AddMassTransit(x =>
            {
                x.AddEntityFrameworkOutbox<AuctionDbContext>(opt =>
                {
                    opt.QueryDelay = TimeSpan.FromSeconds(10);
                    opt.UsePostgres();
                    opt.UseBusOutbox();
                });
                
                x.AddConsumersFromNamespaceContaining<AuctionCreatedFaultConsumer>();
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction", false));
                
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            });
    }

    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        return services.AddSingleton(new MappingProfiles().Mapper);
    }
}