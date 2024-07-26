using BiddingService.Models;

namespace BiddingService.Services;

public interface IBidService
{
    Task<Bid> HandleBidOnAuctionAsync(string auctionId, int incomingBidAmount, string userName);
    Task<IEnumerable<Bid>> GetBidsByAuctionIdAsync(string auctionId);
}