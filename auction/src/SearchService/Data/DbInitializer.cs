﻿using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Services;
using System.Text.Json;

namespace SearchService.Data
{
    public class DbInitializer
    {
        public static async Task InitDb(WebApplication app)
        {
            await DB.InitAsync("SearchDb", MongoClientSettings
                        .FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

            await DB.Index<Item>()
                .Key(x => x.Make, KeyType.Text)
                .Key(x => x.Model, KeyType.Text)
                .Key(x => x.Color, KeyType.Text)
                .CreateAsync();

            var count = await DB.CountAsync<Item>();


            using var scope = app.Services.CreateScope();

            var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();

            var serviceItems = await httpClient.GetItemsForSearchDb();

            Console.WriteLine(serviceItems.Count + " returned from the auction service");

            if (count == 0)
            {


                Console.WriteLine("No data found -- seeding initial data");
                var itemData = await File.ReadAllTextAsync("Data/auctions.json");
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var items = JsonSerializer.Deserialize<List<Item>>(itemData, options);
                await DB.SaveAsync(items);
            }
        }
    }
}
