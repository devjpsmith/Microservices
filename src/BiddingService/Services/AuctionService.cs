using BiddingService.Models;
using BiddingService.Repositories;
using Contracts;

namespace BiddingService.Services;

public class AuctionService : IAuctionService
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IBidRepository _bidRepository;

    public AuctionService(IAuctionRepository auctionRepository, IBidRepository bidRepository)
    {
        _auctionRepository = auctionRepository;
        _bidRepository = bidRepository;
    }

    public async Task<AuctionFinished> ExpireAuction(Auction auction)
    {
        auction.Finished = true;
        await _auctionRepository.UpdateAuction(auction);
        var winningBid = await _bidRepository.GetAuctionWinningBidAsync(auction.ID);
        return new AuctionFinished
        {
            ItemSold = winningBid != null,
            AuctionId = auction.ID,
            Winner = winningBid?.Bidder,
            Seller = auction.Seller,
            Amount = winningBid?.Amount,
        };
    }
}