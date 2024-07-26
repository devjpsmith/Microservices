using AutoMapper;
using BiddingService.DTOs;
using BiddingService.Models;
using Contracts;

namespace BiddingService.Util;

public class MappingService : IAuctionMapper, IBidMapper
{
    private readonly IMapper _mapper;

    public MappingService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Auction MapAuctionCreated(AuctionCreated auctionCreated)
    {
        return _mapper.Map<Auction>(auctionCreated);
    }

    public BidDTO GetBidDTO(Bid bid)
    {
        return _mapper.Map<BidDTO>(bid);
    }

    public BidPlaced GetBidPlaced(Bid bid)
    {
        return _mapper.Map<BidPlaced>(bid);
    }
}