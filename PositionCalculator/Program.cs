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
                coin.buyAmountUsd = ((coin.targetPercentage - coin.currentPercentage) * maximumValue / 100) / 10; //divided by extra 10
                coin.buyAmountCrypto = coin.buyAmountUsd / coin.price;
                buyTotal += coin.buyAmountUsd;
                Console.WriteLine("Values: {0} {1} {2} {3} {4} {5}", coin.names.GeckoName, coin.units, coin.price, coin.currentPercentage, coin.targetPercentage, coin.buyAmountUsd);


                if (coin.exchange == Coin.Exchange.Bitfinex)
                {
                    var symbolData1 = await bitfinexClient.SpotApi.Trading.PlaceOrderAsync(
                        coin.names.BitfinexName,
                        OrderSide.Buy,
                        OrderType.ExchangeMarket,
                        (decimal)coin.buyAmountCrypto,
                        0);
                }
            }
            Console.WriteLine("Total: " + buyTotal);
        }

        static List<Coin> GetPortfolio()
        {
            List<Coin> portfolio = new List<Coin>();
            //portfolio.Add(new Coin("ethereum", "tETHUSD", 2.274294565, 50, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("matic-network", "tMATIC:USD", 67.71867193, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("chainlink", "tLINK:USD", 3.50167336, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("curve-dao-token", "tCRVUSD", 0.00000000, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("frax-share", "", 0.74251200, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("convex-finance", "", 0.00000000, 0.5, Coin.Exchange.GateIO));
            //portfolio.Add(new Coin("0x", "tZRXUSD", 0.00000000, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("helium", "", 71.28820587, 4, Coin.Exchange.GateIO));
            //portfolio.Add(new Coin("oasis-network", "tROSE:USD", 327.48659213, 0.5, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("cosmos", "tATOUSD", 0.00000000, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("avalanche-2", "tAVAX:USD", 4.31795086, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("polkadot", "tDOTUSD", 2.98437031, 0.5, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("near", "tNEAR:USD", 0.00000000, 0.5, Coin.Exchange.Bitfinex));

            //portfolio.Add(new Coin("solana", "tSOLUSD", 4.31453000, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("fantom", "tFTMUSD", 305.09564653, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("thorchain", "", 7.20861000, 0.5, Coin.Exchange.GateIO));
            //portfolio.Add(new Coin("algorand", "tALGUSD", 55.69801400, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("binancecoin", "", 0.24542685, 2, Coin.Exchange.Nexo));
            //portfolio.Add(new Coin("tron", "tTRXUSD", 0.00000000, 0.5, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("uniswap", "tUNIUSD", 0.00000000, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("aave", "tAAVE:USD", 0.00000000, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("zcash", "tZECUSD", 0.29400000, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("dogecoin", "tDOGE:USD", 0.00000000, 0.5, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("the-graph", "tGRTUSD", 0.00000000, 0.5, Coin.Exchange.Bitfinex));

            return portfolio;
        }


    }

}


