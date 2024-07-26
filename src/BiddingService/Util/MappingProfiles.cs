using AutoMapper;
using BiddingService.DTOs;
using BiddingService.Models;
using Contracts;

namespace BiddingService.Util;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<AuctionCreated, Auction>()
            .ForMember(target => target.ID, opt => opt.MapFrom(src => src.Id.ToString()));
        CreateMap<Bid, BidDTO>();
        CreateMap<Bid, BidPlaced>();
    }
}