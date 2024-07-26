using BiddingService.Models;
using Contracts;

namespace BiddingService.Util;

public interface IAuctionMapper
{
    Auction MapAuctionCreated(AuctionCreated auctionCreated);
}