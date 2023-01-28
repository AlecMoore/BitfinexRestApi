using Bitfinex.Net.Clients;
using Bitfinex.Net.Enums;
using Bitfinex.Net.Objects;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Logging;

namespace PositionCalculator
{
    class Program
    {
        private static BitfinexClient bitfinexClient;

        private static void setUp()
        {
            bitfinexClient = new BitfinexClient(new BitfinexClientOptions()
            {
                //ApiCredentials = new ApiCredentials("cVEc61LYLYUtXFxpSf668FuST5m5WfXRkx2M7xgbq4H", "bBqKbdGFT8YVRSvTNBVMPaGaPaBJuq5ZEfbacU97BSH"),
                ApiCredentials = new ApiCredentials("mlYORC0XslzL0ROLqCYBpYWz9HX8TuJOoXgebQbP2u9", "1nTTBGVfjJdx885oVPsRIsP6Aapi8XagpXjOy2FCT7E"),
                LogLevel = LogLevel.Trace
                //RequestTimeout = TimeSpan.FromSeconds(60)
            });
        }

        public static async Task Main(string[] args)
        {
            double maximumValue = 10000.0;
            double buyTotal = 0.0;
            setUp();

            List<Coin> portfolio = GetPortfolio();
            foreach (Coin coin in portfolio)
            {
                coin.price = await CoinGeckoUtils.readPrice(coin.names.GeckoName);
                coin.value = coin.price * coin.units;
                coin.currentPercentage = (coin.value / maximumValue) * 100.0;
                coin.buyAmountUsd = ((coin.targetPercentage - coin.currentPercentage) * maximumValue / 100) / 8;
                coin.buyAmountCrypto = coin.buyAmountUsd / coin.price;
                buyTotal += coin.buyAmountUsd;
                Console.WriteLine("Values: {0} {1} {2} {3} {4} {5}", coin.names.GeckoName, coin.units, coin.price, coin.currentPercentage, coin.targetPercentage, coin.buyAmountUsd);


                //if (coin.exchange == Coin.Exchange.Bitfinex && coin.buyAmountCrypto > 0)
                //{
                //    var symbolData1 = await bitfinexClient.SpotApi.Trading.PlaceOrderAsync(
                //        coin.names.BitfinexName,
                //        OrderSide.Buy,
                //        OrderType.ExchangeMarket,
                //        (decimal)coin.buyAmountCrypto,
                //        0);
                //}
            }
            Console.WriteLine("Total: " + buyTotal);
        }

        static List<Coin> GetPortfolio()
        {
            List<Coin> portfolio = new List<Coin>();
            //portfolio.Add(new Coin("ethereum", "tETHUSD", 2.462230445, 50, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("matic-network", "tMATIC:USD", 79.10220315, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("chainlink", "tLINK:USD", 8.09934835, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("curve-dao-token", "tCRVUSD", 9.50568531, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("frax-share", "", 1.85332300, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("convex-finance", "", 1.55815800, 0.5, Coin.Exchange.GateIO));
            //portfolio.Add(new Coin("0x", "tZRXUSD", 40.19351334, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("helium", "", 98.18146714, 4, Coin.Exchange.GateIO));
            //portfolio.Add(new Coin("oasis-network", "tROSE:USD", 440.38706066, 0.5, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("cosmos", "tATOUSD", 2.11889597, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("avalanche-2", "tAVAX:USD", 4.31824988, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("polkadot", "tDOTUSD", 4.25291411, 0.5, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("near", "tNEAR:USD", 6.09817745, 0.5, Coin.Exchange.Bitfinex));

            //portfolio.Add(new Coin("solana", "tSOLUSD", 8.68162928, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("fantom", "tFTMUSD", 374.15306088, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("thorchain", "", 10.39286300, 0.5, Coin.Exchange.GateIO));
            //portfolio.Add(new Coin("algorand", "tALGUSD", 104.05418900, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("binancecoin", "", 0.31472682, 2, Coin.Exchange.GateIO));
            //portfolio.Add(new Coin("tron", "tTRXUSD", 90.77846575, 0.5, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("uniswap", "tUNIUSD", 3.44442153, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("aave", "tAAVE:USD", 0.19084252, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("zcash", "tZECUSD", 0.80426410, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("dogecoin", "tDOGE:USD", 102.40474022, 0.5, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("the-graph", "tGRTUSD", 89.50030490, 0.5, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("aptos", "tAPTUSD", 3.79240000, 0.5, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("havven", "tSNXUSD", 0.00000000, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("optimism", "", 0.00000000, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("dydx", "", 0.00000000, 0.5, Coin.Exchange.GateIO));

            return portfolio;
        }


    }

}


