using BiddingService.Models;
using Contracts;

namespace BiddingService.Services;

public interface IAuctionService
{
    Task<AuctionFinished> ExpireAuction(Auction auction);
}