using AuctionService.Data;
using AuctionService.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers.Internal;
/*
 * This is a rather contrived controller. I made it simply for the purpose of demonstrating scoped endpoints.
 * The /.internal/ endpoints require the "internal" scope, which user tokens do not have
 */
[Authorize("internal")]
[ApiController]
[Route(".internal/[controller]")]
public class AuctionsController : Controller
{
    private readonly AuctionDbContext _dbContext;
    private readonly IMapper _mapper;

    public AuctionsController(AuctionDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ActionResult<IList<AuctionDto>>> Get()
    {
        var query = _dbContext.Auctions.OrderBy(x => x.Item.Make)
            .AsQueryable();
        
        return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
    }
}