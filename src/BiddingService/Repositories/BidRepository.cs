using BiddingService.Models;
using MongoDB.Entities;

namespace BiddingService.Repositories;

public class BidRepository : IBidRepository
{
    public async Task<IEnumerable<Bid>> GetAuctionBidsAsync(string auctionId)
    {
        var bids = await DB.Find<Bid>()
            .Match(x => x.AuctionId == auctionId)
            .Sort(x => x.Descending(y => y.BidTime))
            .ExecuteAsync();

        return bids.AsEnumerable();
    }

    public Task<Bid> GetAuctionHighestBidAsync(string auctionId)
    {
        return DB.Find<Bid>()
            .Match(x => x.AuctionId == auctionId)
            .Sort(x => x.Descending(y => y.Amount))
            .ExecuteFirstAsync();
    }

    public Task<Bid> GetAuctionWinningBidAsync(string auctionId)
    {
        return DB.Find<Bid>()
            .Match(x => x.AuctionId == auctionId)
            .Match(x => x.BidStatus == BidStatus.Accepted)
            .Sort(x => x.Descending(y => y.Amount))
            .ExecuteFirstAsync();
    }

    public Task InsertAsync(Bid bid)
    {
        return DB.SaveAsync(bid);
    }
}