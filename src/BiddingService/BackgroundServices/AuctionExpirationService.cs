using BiddingService.Repositories;
using BiddingService.Services;
using Contracts;
using MassTransit;

namespace BiddingService.BackgroundServices;

public class AuctionExpirationService : BackgroundService
{
    private readonly ILogger<AuctionExpirationService> _logger;
    private readonly IServiceProvider _services;

    public AuctionExpirationService(IServiceProvider services, ILogger<AuctionExpirationService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting check for finished auctions");

        stoppingToken.Register(() => _logger.LogInformation("==> Auction check is stopping"));

        using (var scope = _services.CreateScope())
        {
            var auctionService = scope.ServiceProvider.GetRequiredService<IAuctionService>();
            var auctionRepository = scope.ServiceProvider.GetRequiredService<IAuctionRepository>();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
            
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckAuctions(auctionService, auctionRepository, publishEndpoint, stoppingToken);

                await Task.Delay(5000, stoppingToken);
            }
        }
    }

    private async Task CheckAuctions(IAuctionService auctionService, 
        IAuctionRepository auctionRepository,
        IPublishEndpoint endpoint,
        CancellationToken stoppingToken)
    {
        var finishedAuctions = await auctionRepository.GetExpiredAuctions();

        foreach (var auction in finishedAuctions)
        {
            var auctionFinished = await auctionService.ExpireAuction(auction);
            await endpoint.Publish(auctionFinished, stoppingToken);
        }
        
        _logger.LogInformation("==> Found {count} auctions that have completed", finishedAuctions.Count());
    }
}