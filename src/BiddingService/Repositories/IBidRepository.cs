using BiddingService.Models;

namespace BiddingService.Repositories;

public interface IBidRepository : IDbRepository<Bid>
{
    Task<IEnumerable<Bid>> GetAuctionBidsAsync(string auctionId);
    Task<Bid> GetAuctionHighestBidAsync(string auctionId);
    Task<Bid> GetAuctionWinningBidAsync(string auctionId);
}