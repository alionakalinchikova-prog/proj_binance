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

        Console.WriteLine("Выбери монеты (введи номера через запятую, например: 1,3,5):");
        foreach (var coin in coins)
        {
            Console.WriteLine($"{coin.Key}. {coin.Value}");
        }
        Console.Write("\nТвой выбор: ");
        string input = Console.ReadLine();

        string[] selectedNumbers = input.Split(',');
        List<string> selectedCoins = new List<string>();

        foreach (string num in selectedNumbers)
        {
            string trimmed = num.Trim();
            if (coins.ContainsKey(trimmed))
            {
                selectedCoins.Add(coins[trimmed]);
            }
            else
            {
                Console.WriteLine($"Номер {trimmed} не найден, пропускаем");
            }
        }

        if (selectedCoins.Count == 0)
        {
            Console.WriteLine("Не выбрано ни одной монеты!");
            return;
        }

        using (HttpClient client = new HttpClient())
        {
            Console.WriteLine("\n КУРСЫ:");
            Console.WriteLine(new string('-', 40));

            foreach (string coinName in selectedCoins)
            {
                string symbol = coinName + "USDT";
                string url = $"https://api.binance.com/api/v3/ticker/price?symbol={symbol}";

                try
                {
                    string json = await client.GetStringAsync(url);
                    using JsonDocument doc = JsonDocument.Parse(json);
                    JsonElement root = doc.RootElement;
                    string price = root.GetProperty("price").GetString();

                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {coinName}: {price} $");

                    string fileName = $"{coinName}_{DateTime.Now:yyyy-MM-dd_HHmm}.json";
                    System.IO.File.WriteAllText(fileName, json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" {coinName}: ошибка - {ex.Message}");
                }
            }

            Console.WriteLine($"\n Сохранено {selectedCoins.Count} файлов");
        }
    }
}