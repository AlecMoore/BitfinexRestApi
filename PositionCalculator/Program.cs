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
                ApiCredentials = new ApiCredentials("cVEc61LYLYUtXFxpSf668FuST5m5WfXRkx2M7xgbq4H", "bBqKbdGFT8YVRSvTNBVMPaGaPaBJuq5ZEfbacU97BSH"),
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
                coin.buyAmountUsd = ((coin.targetPercentage - coin.currentPercentage) * maximumValue / 100) / 10; //divided by extra 10
                coin.buyAmountCrypto = coin.buyAmountUsd / coin.price;
                buyTotal += coin.buyAmountUsd;
                Console.WriteLine("Values: {0} {1} {2} {3} {4} {5}", coin.names.GeckoName, coin.units, coin.price, coin.currentPercentage, coin.targetPercentage, coin.buyAmountUsd);

                //if(coin.exchange == Coin.Exchange.Bitfinex)
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
            portfolio.Add(new Coin("ethereum", "tETHUSD", 2.294620759, 50, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("matic-network", "tMATIC:USD", 51.27097369, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("chainlink", "tLINK:USD", 4.90139336, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("curve-dao-token", "tCRVUSD", 8.99722400, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("frax-share", "", 2.09950240, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("convex-finance", "", 1.29962240, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("0x", "tZRXUSD", 65.98700400, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("helium", "",  44.17984019, 4, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("oasis-network", "tROSE:USD", 284.89242247, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("cosmos", "tATOUSD", 1.07834922, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("avalanche-2", "tAVAX:USD", 4.92273822, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("polkadot", "tDOTUSD", 2.08166579, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("near", "tNEAR:USD", 4.09918000, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("solana", "tSOLUSD", 2.35134979, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("fantom", "tFTMUSD", 306.37004673, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("thorchain", "", 8.52681662, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("algorand", "tALGUSD", 37.54453200, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("binancecoin", "", 0.19976722, 2, Coin.Exchange.Nexo));
            portfolio.Add(new Coin("tron", "tTRXUSD", 443.84440709, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("uniswap", "tUNIUSD", 0.0, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("aave", "tAAVE:USD", 0.0, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("zcash", "tZECUSD", 0.0, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("dogecoin", "tDOGE:USD", 0.0, 0.5, Coin.Exchange.Bitfinex));

            return portfolio;
        }


    }

}


