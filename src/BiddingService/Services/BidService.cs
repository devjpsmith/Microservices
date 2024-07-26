using BiddingService.Models;
using BiddingService.Repositories;
using BiddingService.Util;

namespace BiddingService.Services;

public class BidService : IBidService
{
    private readonly IBidStatusCalculator _statusCalculator;
    private readonly IAuctionRepository _auctionRepository;
    private readonly IBidRepository _bidRepository;
    private readonly GrpcAuctionClient _grpcAuctionClient;

    public BidService(IBidStatusCalculator statusCalculator, 
        IAuctionRepository auctionRepository, 
        IBidRepository bidRepository, 
        GrpcAuctionClient grpcAuctionClient)
    {
        _statusCalculator = statusCalculator;
        _auctionRepository = auctionRepository;
        _bidRepository = bidRepository;
        _grpcAuctionClient = grpcAuctionClient;
    }


    public async Task<Bid> HandleBidOnAuctionAsync(string auctionId, int incomingBidAmount, string userName)
    {
        var auction = await _auctionRepository.GetAuctionAsync(auctionId);
        
        if (auction == null)
        {
            auction = _grpcAuctionClient.GetAuction(auctionId)
                ?? throw new KeyNotFoundException();
        }
        
        if (auction.Seller == userName)
        {
            throw new ArgumentException("Users cannot bid on auctions they created");
        }
        
        var bid = new Bid
        {
            Amount = incomingBidAmount,
            AuctionId = auctionId,
            Bidder = userName
        };

        var highBid = await _bidRepository.GetAuctionHighestBidAsync(auctionId);

        bid.BidStatus = _statusCalculator.CalculateBidStatus(incomingBidAmount, auction, highBid?.Amount);

        await _bidRepository.InsertAsync(bid);

        return bid;
    }

    public Task<IEnumerable<Bid>> GetBidsByAuctionIdAsync(string auctionId)
    {
        return _bidRepository.GetAuctionBidsAsync(auctionId);
    }
}