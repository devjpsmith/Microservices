using BiddingService.Models;

namespace BiddingService.Util;

public interface IBidStatusCalculator
{
    BidStatus CalculateBidStatus(int incomingBid, Auction auction, int? highBidAmount);
}