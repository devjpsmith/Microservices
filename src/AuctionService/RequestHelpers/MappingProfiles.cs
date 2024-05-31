using AuctionService.DTO;
using AutoMapper;
using AuctionService.Entities;

namespace AuctionService.RequestHelpers;

public class MappingProfiles
{
    public readonly IMapper Mapper;
    public MappingProfiles()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Auction, AuctionDto>()
                .IncludeMembers(x => x.Item);
            cfg.CreateMap<Item, AuctionDto>();
            cfg.CreateMap<CreateAuctionDto, Auction>()
                .ForMember(dest => dest.Item, option => option.MapFrom(src => src));
            cfg.CreateMap<CreateAuctionDto, Item>();
        });
        Mapper = config.CreateMapper();
    }
}