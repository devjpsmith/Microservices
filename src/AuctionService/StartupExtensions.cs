using AuctionService.Data;
using AuctionService.RequestHelpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AuctionService;

public static class StartupExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        return services.AddDbContext<AuctionDbContext>(options =>
        {
            options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
        });
    }

    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        return services.AddSingleton<IMapper>(new MappingProfiles().Mapper);
    }
}