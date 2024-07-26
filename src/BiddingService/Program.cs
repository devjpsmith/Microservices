using BiddingService;
using BiddingService.BackgroundServices;
using BiddingService.Services;
using MongoDB.Driver;
using MongoDB.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.ConfigureAuthentication(builder.Configuration)
    .ConfigureMassTransit(builder.Configuration)
    .ConfigureServices();
builder.Services.AddHostedService<AuctionExpirationService>();
builder.Services.AddScoped<GrpcAuctionClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseAuthentication()
    .UseAuthorization();

app.MapControllers();

await DB.InitAsync("BidDB",
    MongoClientSettings.FromConnectionString(builder.Configuration.GetConnectionString("BidDBConn")));

app.Run();
