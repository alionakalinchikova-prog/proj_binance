using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

class Program
{
    static async Task Main()
    {
        using (HttpClient client = new HttpClient())
        {
            string url = "https://api.binance.com/api/v3/ticker/price?symbol=BTCUSDT";
            string json = await client.GetStringAsync(url);

            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;

            string price = root.GetProperty("price").GetString();
            Console.WriteLine($"BTC: {price} $");
        }
    }
}