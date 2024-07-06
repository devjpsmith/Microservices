using AuctionService.Authorization;
using AuctionService.Consumers;
using AuctionService.Data;
using AuctionService.RequestHelpers;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AuctionService;

public static class StartupExtensions
{
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
        services.AddAuthorization(opts =>
        {
            // this policy is scoped "internal". In our system here, this allows endpoints to be restricted. M2M tokens 
            // have the "internal" scope while user tokens do not
            opts.AddPolicy("internal", policy => policy.Requirements.Add(new HasScopeRequirement("internal", authority)));
        });

        services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
        
        return services;
    }


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

                x.UsingRabbitMq((context, cfg) => { cfg.ConfigureEndpoints(context); });
            });
    }

    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        return services.AddSingleton(new MappingProfiles().Mapper);
    }
}