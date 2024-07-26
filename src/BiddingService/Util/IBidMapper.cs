using BiddingService.DTOs;
using BiddingService.Models;
using Contracts;

namespace BiddingService.Util;

public interface IBidMapper
{
    BidDTO GetBidDTO(Bid bid);

    BidPlaced GetBidPlaced(Bid bid);
}