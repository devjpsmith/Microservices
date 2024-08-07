using AuctionService.Data;
using Grpc.Core;

namespace AuctionService.GrpcServices;

public class GrpcAuctionService : GrpcAuction.GrpcAuctionBase
{
    private readonly AuctionDbContext _dbContext;

    public GrpcAuctionService(AuctionDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<GrpcAuctionResponse> GetAuction(GetAuctionRequest request, ServerCallContext context)
    {
        Console.WriteLine("==> Received Grpc request for GetAuction");

        var auction = await _dbContext.Auctions.FindAsync(Guid.Parse(request.Id));

        if (auction == null) throw new RpcException(new Status(StatusCode.NotFound, "Not found"));

        var response = new GrpcAuctionResponse
        {
            Auction = new()
            {
                AuctionEnd = auction.AuctionEnd.ToString(),
                Id = auction.Id.ToString(),
                ReservePrice = auction.ReservePrice,
                Seller = auction.Seller
            }
        };

        return response;
    }
}