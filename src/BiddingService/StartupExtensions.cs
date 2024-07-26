using BiddingService.Consumers;
using BiddingService.Repositories;
using BiddingService.Services;
using BiddingService.Util;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BiddingService;

public static class StartupExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
        => services.AddTransient<IBidStatusCalculator, BidStatusCalculator>()
            .AddTransient<IBidService, BidService>()
            .AddTransient<IAuctionService, Services.AuctionService>()
            .AddTransient<IAuctionRepository, AuctionRepository>()
            .AddTransient<IBidRepository, BidRepository>()
            .AddTransient<IAuctionMapper, MappingService>()
            .AddTransient<IBidMapper, MappingService>();
    
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var authority = config.GetSection("Authentication").GetValue<string>("Authority");
        var audience = config.GetSection("Authentication").GetValue<string>("Audience");
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opts =>
            {
                
                opts.Authority = authority;
                opts.Audience = audience;
                opts.RequireHttpsMetadata = false;
                opts.TokenValidationParameters.ValidateAudience = true;
                opts.TokenValidationParameters.NameClaimType = "username";
            });
        
        return services;
    }

    public static IServiceCollection ConfigureMassTransit(this IServiceCollection services, IConfiguration config)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
            
            x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("bids", false));

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