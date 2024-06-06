using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;

namespace SearchService.Data;

public class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        var connStr = app.Configuration.GetConnectionString("MongoDbConnection");
        await DB.InitAsync("SearchDb", MongoClientSettings.FromConnectionString(connStr));

        await DB.Index<Item>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .CreateAsync();

        var date = await DB.Find<Item, string>()
            .Sort(x => x.Descending(item => item.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();
        
        var client = app.Services.GetRequiredService<AuctionSvcHttpClient>();
        var items = await client.GetItems(date);
        if (items.Count > 0)
            await DB.SaveAsync(items);
    }
}