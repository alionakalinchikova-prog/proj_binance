using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("КУРСЫ КРИПТОВАЛЮТ (BINANCE)");
        Console.WriteLine(new string('-', 40));

        var coins = new Dictionary<string, string>
        {
            { "1", "BTC" },
            { "2", "ETH" },
            { "3", "BNB" },
            { "4", "SOL" },
            { "5", "XRP" },
            { "6", "ADA" },
            { "7", "DOGE" },
            { "8", "DOT" }
        };

        Console.WriteLine("Выбери монету:");
        foreach (var coin in coins)
        {
            Console.WriteLine($"{coin.Key}. {coin.Value}");
        }
        Console.Write("Введите номер (1-8): ");
        string choice = Console.ReadLine();

        if (!coins.ContainsKey(choice))
        {
            Console.WriteLine("Неверный выбор!");
            return;
        }

        string coinName = coins[choice];
        string symbol = coinName + "USDT";

        using (HttpClient client = new HttpClient())
        {
            string url = $"https://api.binance.com/api/v3/ticker/price?symbol={symbol}";
            string json = await client.GetStringAsync(url);
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;
            string price = root.GetProperty("price").GetString();

            Console.WriteLine($"\n{coinName}: {price} $");

            string fileName = $"{coinName}_price_{DateTime.Now:yyyy-MM-dd_HHmm}.json";
            System.IO.File.WriteAllText(fileName, json);
            Console.WriteLine($"Сохранено в {fileName}");
        }
    }
}