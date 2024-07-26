using BiddingService.Repositories;
using BiddingService.Util;
using Contracts;
using MassTransit;

namespace BiddingService.Consumers;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IAuctionMapper _auctionMapper;

    public AuctionCreatedConsumer(IAuctionRepository auctionRepository, IAuctionMapper auctionMapper)
    {
        _auctionRepository = auctionRepository;
        _auctionMapper = auctionMapper;
    }

    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine("--> Consuming AuctionCreated");
        var auction = _auctionMapper.MapAuctionCreated(context.Message);
        await _auctionRepository.InsertAsync(auction);
    }
}