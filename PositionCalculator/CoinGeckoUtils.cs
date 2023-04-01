using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static PositionCalculator.Coin;

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


        public static async Task<List<Coin>> readAllPrice(List<Coin> portfolio)
        {

            string allCoins = "";
            foreach(Coin c in portfolio) {
                allCoins += "%2C" + c.names.GeckoName;
            }
            allCoins = allCoins.Substring(3);

            HttpClient client = new HttpClient();

            String json = await client.GetStringAsync("https://api.coingecko.com/api/v3/simple/price?ids=" + allCoins + "&vs_currencies=usd");

            var jo = JObject.Parse(json);

            foreach (Coin c in portfolio)
            {
                try
                {
                    c.price = (double)jo[c.names.GeckoName]["usd"];
                } catch (Exception e)
                {
                    c.price = 0.0;
                }
            }

            return portfolio;
        }
    }
}
