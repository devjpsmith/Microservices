using Polly;
using Polly.Extensions.Http;
using SearchService.Data;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddTransient<AuctionSvcHttpClient>();
builder.Services.AddHttpClient(nameof(AuctionSvcHttpClient), client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AuctionServiceUrl"]);
    client.Timeout = TimeSpan.FromMinutes(100);
}).AddPolicyHandler(GetPolicy());

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async () =>
{
    try
    {
        await DbInitializer.InitDb(app);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
});


app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions.HandleTransientHttpError()
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));