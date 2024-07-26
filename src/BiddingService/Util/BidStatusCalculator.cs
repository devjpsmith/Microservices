using BiddingService.Models;

namespace BiddingService.Util;

public class BidStatusCalculator : IBidStatusCalculator
{
    public BidStatus CalculateBidStatus(int incomingBid, Auction auction, int? highBidAmount)
    {
        if (auction.AuctionEnd < DateTime.UtcNow) return BidStatus.Finished;
        
        if (highBidAmount != null && incomingBid > highBidAmount || highBidAmount == null)
        {
            return incomingBid > auction.ReservePrice
                ? BidStatus.Accepted
                : BidStatus.AcceptedBelowReserve;
        }

        if (highBidAmount != null && incomingBid <= highBidAmount)
        {
            return BidStatus.TooLow;
        }

        return BidStatus.Accepted;
    }
}