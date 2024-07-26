using BiddingService.Models;
using MongoDB.Entities;

namespace BiddingService.Repositories;

public class AuctionRepository : IAuctionRepository
{
    public Task<Auction> GetAuctionAsync(string auctionId)
    {
        return DB.Find<Auction>()
            .OneAsync(auctionId);
    }

    public async Task<IEnumerable<Auction>> GetExpiredAuctions()
    {
        var auctions = await DB.Find<Auction>()
            .Match(x => x.AuctionEnd < DateTime.UtcNow)
            .Match(x => !x.Finished)
            .ExecuteAsync();

        return auctions.AsEnumerable();
    }

    public Task UpdateAuction(Auction auction)
    {
        return auction.SaveAsync();
    }

    public Task InsertAsync(Auction auction)
    {
        return DB.SaveAsync(auction);
    }
}