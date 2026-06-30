using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

class Program
{
    static async Task Main()
    {
        Console.WriteLine(" Курс криптовалюты к доллару"); 
        Console.WriteLine(new string('-', 30));

        using (HttpClient client = new HttpClient())
        {
            //string url = "https://api.binance.com/api/v3/ticker/price?symbol=BTCUSDT"; //биткоин
            string url = "https://api.binance.com/api/v3/ticker/price?symbol=ETHUSDT";  // эфир
            //string url = "https://api.binance.com/api/v3/ticker/price?symbol=BNBUSDT";   // BNB
           //string url = "https://api.binance.com/api/v3/ticker/price?symbol=DOGEUSDT";  // догикойн*

            string json = await client.GetStringAsync(url);

            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;

            string symbol = root.GetProperty("symbol").GetString();
            string price = root.GetProperty("price").GetString();

            Console.WriteLine($"\n{symbol}: {price} $");

            string fileName = $"price_{DateTime.Now:yyyy-MM-dd_HHmm}.json";
            System.IO.File.WriteAllText(fileName, json);
            Console.WriteLine($"\n Сохранено в {fileName}");
        }
    }
}