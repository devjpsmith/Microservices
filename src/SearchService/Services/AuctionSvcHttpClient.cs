using SearchService.Models;

namespace SearchService.Services;

public class AuctionSvcHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AuctionSvcHttpClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<Item>> GetItems(string date)
    {
        var client = _httpClientFactory.CreateClient(nameof(AuctionSvcHttpClient));
        return await client.GetFromJsonAsync<List<Item>>($"api/Auctions/?date={date}");
    }
}