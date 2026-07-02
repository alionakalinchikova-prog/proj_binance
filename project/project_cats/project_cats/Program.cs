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
        Console.WriteLine(new string('-', 50));

        var coins = new Dictionary<string, string>
        {
            { "1", "BTC" },
            { "2", "ETH" },
            { "3", "BNB" },
            { "4", "SOL" },
            { "5", "XRP" },
            { "6", "ADA" },
            { "7", "DOGE" },
            { "8", "DOT" },
            { "9", "AVAX" }
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
            Console.WriteLine("\nКУРСЫ И СТАТИСТИКА:");
            Console.WriteLine(new string('-', 50));

            foreach (string coinName in selectedCoins)
            {
                string symbol = coinName + "USDT";

                string urlPrice = $"https://api.binance.com/api/v3/ticker/price?symbol={symbol}";
                string jsonPrice = await client.GetStringAsync(urlPrice);
                using JsonDocument docPrice = JsonDocument.Parse(jsonPrice);
                string price = docPrice.RootElement.GetProperty("price").GetString();

                string url24hr = $"https://api.binance.com/api/v3/ticker/24hr?symbol={symbol}";
                string json24hr = await client.GetStringAsync(url24hr);
                using JsonDocument doc24hr = JsonDocument.Parse(json24hr);
                JsonElement root24 = doc24hr.RootElement;

                string high = root24.GetProperty("highPrice").GetString();
                string low = root24.GetProperty("lowPrice").GetString();
                string change = root24.GetProperty("priceChangePercent").GetString();

                Console.WriteLine($"\n{coinName}:");
                Console.WriteLine($"  Цена: {price} $");
                Console.WriteLine($"  Максимум за 24ч: {high} $");
                Console.WriteLine($"  Минимум за 24ч: {low} $");
                Console.WriteLine($"  Изменение за 24ч: {change} %");

                var combined = new
                {
                    coin = coinName,
                    symbol = symbol,
                    timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    price = jsonPrice,
                    stats24h = json24hr
                };
                string combinedJson = JsonSerializer.Serialize(combined, new JsonSerializerOptions { WriteIndented = true });
                string fileName = $"{coinName}_{DateTime.Now:yyyyMMdd_HHmm}.json";
                System.IO.File.WriteAllText(fileName, combinedJson);
            }

            Console.WriteLine($"\nСохранено {selectedCoins.Count} файлов");
        }
    }
}