using AuctionService.Data;
using AuctionService.DTO;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuctionsController : Controller
{
    private readonly AuctionDbContext _db;
    private readonly IMapper _mapper;

    public AuctionsController(AuctionDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuctionDto>>> Get(string? date)
    {
        var query = _db.Auctions.OrderBy(x => x.Item.Make)
            .AsQueryable();

        if (!string.IsNullOrEmpty(date))
            query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);

        return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetById([FromRoute] Guid id)
    {
        var auction = await _db.Auctions
            .Include(x => x.Item)
                    .FirstOrDefaultAsync(x => x.Id == id);

        if (auction == null) return NotFound();
        return _mapper.Map<AuctionDto>(auction);
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto createAuctionDto)
    {
        var auction = _mapper.Map<Auction>(createAuctionDto);
        // TODO: add current user as seller
        auction.Seller = "test";
        _db.Auctions.Add(auction);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new {auction.Id}, _mapper.Map<AuctionDto>(auction));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuction)
    {
        var auction = await _db.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (auction == null) return new NotFoundResult();
        
        // TODO: check seller == user
        auction.Item.Make = updateAuction.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuction.Model ?? auction.Item.Model;
        auction.Item.Color = updateAuction.Color ?? auction.Item.Color;
        auction.Item.Mileage = updateAuction.Mileage ?? auction.Item.Mileage;
        auction.Item.Year = updateAuction.Year ?? auction.Item.Year;

        var results = await _db.SaveChangesAsync() > 0;

        if (results) return Ok();

        return BadRequest("Problem saving changes");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        var auction = await _db.Auctions.FirstOrDefaultAsync(x => x.Id == id);

        if (auction == null) return NotFound();
        
        // TODO: check seller == user

        _db.Auctions.Remove(auction);

        var result = await _db.SaveChangesAsync() > 0;

        if (!result) return BadRequest();

        return Ok();
    }
}