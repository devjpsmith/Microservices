using BiddingService.DTOs;
using BiddingService.Models;
using BiddingService.Services;
using BiddingService.Util;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiddingService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BidsController : ControllerBase
{
    private readonly IBidService _bidService;
    private readonly IBidMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public BidsController(IBidService bidService,
        IBidMapper mapper, 
        IPublishEndpoint publishEndpoint)
    {
        _bidService = bidService;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet("{auctionId}")]
    public async Task<ActionResult<List<BidDTO>>> GetBidsByAuctionId([FromRoute] string auctionId)
    {
        var bids = await _bidService.GetBidsByAuctionIdAsync(auctionId);
        return Ok(bids.Select(_mapper.GetBidDTO));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BidDTO>> PlaceBid(string auctionId, int amount)
    {
        try
        {
            var bidResult = await _bidService.HandleBidOnAuctionAsync(auctionId, amount, User.Identity.Name);
            await _publishEndpoint.Publish(_mapper.GetBidPlaced(bidResult));
            return Ok(_mapper.GetBidDTO(bidResult));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
}