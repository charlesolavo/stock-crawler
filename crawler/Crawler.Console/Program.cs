using AutoTest.Pages;
using MongoDB.Driver;
using System;

namespace Crawler.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            GetStocksPage page = new GetStocksPage();
            page.CarregarPagina();
            var stocks = page.GetStocks();
            page.Fechar();

            var client = new MongoClient((string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MONGODB_URL"))) ? "mongodb://mongouser:mongopwd@localhost:27017/admin" : Environment.GetEnvironmentVariable("MONGODB_URL"));
            var database = client.GetDatabase("admin");
            var collection = database.GetCollection<StockRegister>("StockRegister");
            collection.InsertOne(new StockRegister() { DateTime = DateTime.Now, Stocks = stocks });
        }
    }
}
