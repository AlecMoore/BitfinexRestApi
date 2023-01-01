using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PositionCalculator
{
    class CoinGeckoUtils
    {
        public static async Task<double> readPrice(string coin)
        {
            HttpClient client = new HttpClient();

            String json = await client.GetStringAsync("https://api.coingecko.com/api/v3/simple/price?ids=" + coin + "&vs_currencies=usd");

            var jo = JObject.Parse(json);

            double price = (double)jo[coin]["usd"];

            return price;
        }

    }
}
