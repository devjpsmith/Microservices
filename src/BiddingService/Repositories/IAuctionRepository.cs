using BiddingService.Models;

namespace BiddingService.Repositories;

public interface IAuctionRepository : IDbRepository<Auction>
{
    Task<Auction> GetAuctionAsync(string auctionId);
    Task<IEnumerable<Auction>> GetExpiredAuctions();
    Task UpdateAuction(Auction auction);
}