using Bitfinex.Net.Clients;
using Bitfinex.Net.Enums;
using Bitfinex.Net.Objects;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Logging;
using Io.Gate.GateApi.Api;
using Io.Gate.GateApi.Client;
using Io.Gate.GateApi.Model;
using System.Globalization;
using System.Diagnostics;
using Binance.Net.Clients;
using Binance.Net.Objects;
using Binance.Net.Enums;

namespace PositionCalculator
{
    class Program
    {
        private static BitfinexClient bitfinexClient;
        private static BinanceClient binanceClient;
        private static SpotApi gateSpotApi;

        private static async void setUp()
        {
            bitfinexClient = new BitfinexClient(new BitfinexClientOptions()
            {
                ApiCredentials = new ApiCredentials("", ""),
                LogLevel = LogLevel.Trace
                //RequestTimeout = TimeSpan.FromSeconds(60)
            });

            binanceClient = new BinanceClient(new BinanceClientOptions()
            {
                ApiCredentials = new BinanceApiCredentials("", ""),
                LogLevel = LogLevel.Trace
                //RequestTimeout = TimeSpan.FromSeconds(60)
            });

            Configuration gateConfig = new Configuration
            {
                // Setting basePath is optional. It defaults to https://api.gateio.ws/api/v4
                //BasePath = _runConfig.HostUsed,
                ApiV4Key = "",
                ApiV4Secret = ""
            };
            gateSpotApi = new SpotApi(gateConfig);
        }

        public static async Task Main(string[] args)
        {
            double maximumValue = 10000.0;
            double buyTotal = 0.0;
            double gateIO = 0.0;
            double binance = 0.0;
            double value = 0.0;
            setUp();

            List<Coin> portfolio = GetPortfolio();
            portfolio = await CoinGeckoUtils.readAllPrice(portfolio);
            foreach (Coin coin in portfolio)
            {
                if (coin.price == 0)
                {
                    coin.price = await CoinGeckoUtils.readPrice(coin.names.GeckoName);
                }
                
                coin.value = coin.price * coin.units;
                coin.currentPercentage = (coin.value / maximumValue) * 100.0;
                coin.buyAmountUsd = ((coin.targetPercentage - coin.currentPercentage) * maximumValue / 100);
                coin.buyAmountCrypto = coin.buyAmountUsd / coin.price;
                value += coin.value;

                if (coin.buyAmountCrypto > 0)
                {
                    buyTotal += coin.buyAmountUsd;
                    Console.WriteLine("Values: {0} {1} {2} {3} {4} {5}", coin.names.GeckoName, coin.units, coin.price, coin.currentPercentage, coin.targetPercentage, coin.buyAmountUsd);
                    if (coin.exchange == Coin.Exchange.GateIO)
                    {
                        gateIO += coin.buyAmountUsd;
                    }                
                    if (coin.exchange == Coin.Exchange.Binance)
                    {
                        binance += coin.buyAmountUsd;
                    }

                    //if (coin.exchange == Coin.Exchange.Bitfinex)
                    //{
                    //    var symbolData1 = await bitfinexClient.SpotApi.Trading.PlaceOrderAsync(
                    //        coin.names.ApiCoinName,
                    //        Bitfinex.Net.Enums.OrderSide.Buy,
                    //        OrderType.ExchangeMarket,
                    //        (decimal)coin.buyAmountCrypto,
                    //        0);
                    //}

                    //if (coin.exchange == Coin.Exchange.Binance)
                    //{
                    //    var was = (decimal)Math.Round(coin.buyAmountCrypto, 2);
                    //    var symbolData1 = await binanceClient.SpotApi.Trading.PlaceOrderAsync(
                    //        coin.names.ApiCoinName,
                    //        Binance.Net.Enums.OrderSide.Buy,
                    //        SpotOrderType.Market,
                    //        quoteQuantity: (decimal)Math.Round(coin.buyAmountUsd, 2));
                    //}

                    //if (coin.exchange == Coin.Exchange.GateIO)
                    //{
                    //    try
                    //    {
                    //        List<Ticker> tickers = gateSpotApi.ListTickers(coin.names.ApiCoinName);
                    //        string lastPrice = tickers[0].Last;

                    //        Order order = new Order(currencyPair: coin.names.ApiCoinName, amount: coin.buyAmountCrypto.ToString(CultureInfo.InvariantCulture), price: lastPrice)
                    //        {
                    //            Account = Order.AccountEnum.Spot,
                    //            Side = Order.SideEnum.Buy,
                    //        };

                    //        var sds = await gateSpotApi.CreateOrderAsync(order);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine($"{ex.Message}");
                    //    }

                    //}
                }
            }
            Console.WriteLine("GateIO: " + gateIO);
            Console.WriteLine("Binance: " + binance);
            Console.WriteLine("Total: " + buyTotal);
            Console.WriteLine("Value: " + value);
        }

        static List<Coin> GetPortfolio()
        {
            List<Coin> portfolio = new List<Coin>();
            portfolio.Add(new Coin("bitcoin", "tETHUSD", 0.17292664, 50, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("ethereum", "tETHUSD", 3.101171343, 50, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("matic-network", "tMATIC:USD", 364.87381388, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("chainlink", "tLINK:USD", 33.32994776, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("curve-dao-token", "tCRVUSD", 169.83637370, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("frax-share", "FXSUSDT", 7.39154800, 0.5, Coin.Exchange.Binance));
            portfolio.Add(new Coin("convex-finance", "CVXUSDT", 16.32051300, 0.5, Coin.Exchange.Binance));
            portfolio.Add(new Coin("olympus", "tZRXUSD", 0.05533350, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("tribe-2", "tZRXUSD", 85.72410000, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("0x", "tZRXUSD", 300.21312070, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("helium", "HNT_USDT", 132.06654007, 2, Coin.Exchange.GateIO)); 
            portfolio.Add(new Coin("defichain", "tROSE:USD", 37.34664000, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("oasis-network", "tROSE:USD", 566.40569446, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("cosmos", "tATOUSD", 44.45820000, 3, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("avalanche-2", "tAVAX:USD", 20.21613107, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("polkadot", "tDOTUSD", 11.78580000, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("near", "tNEAR:USD", 44.61300000, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("solana", "tSOLUSD", 15.42388588, 3, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("fantom", "tFTMUSD", 983.05132200, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("thorchain", "RUNEUSDT", 41.97860800, 0.5, Coin.Exchange.Binance));
            portfolio.Add(new Coin("algorand", "tALGUSD", 536.86649900, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("binancecoin", "BNBUSDT", 0.92521083, 2, Coin.Exchange.Binance));
            portfolio.Add(new Coin("tron", "tTRXUSD", 649.81800000, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("flow", "tUNIUSD", 45.15505740, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("uniswap", "tUNIUSD", 45.15505740, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("aave", "tAAVE:USD", 3.63746795, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("zcash", "tZECUSD", 12.26270000, 3, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("dogecoin", "tDOGE:USD", 788.41200000, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("gmx", "GMXUSDT", 2.97038100, 1, Coin.Exchange.Binance));
            portfolio.Add(new Coin("the-graph", "tGRTUSD", 558.86075640, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("aptos", "tAPTUSD", 8.87551260, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("snowball-token", "tSNXUSD", 2.40395126, 1, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("wbnb", "tSNXUSD", 0.00477883, 1, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("wrapped-fantom", "tSNXUSD", 0.36648420, 1, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("havven", "tSNXUSD", 50.14545026, 1, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("optimism", "OPUSDT", 36.70961862, 0.5, Coin.Exchange.Binance));
            portfolio.Add(new Coin("dydx", "DYDXUSDT", 47.58993000, 1, Coin.Exchange.Binance));
            portfolio.Add(new Coin("radix", "tXRDUSD", 1220.56571655, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("singularitynet", "AGIXUSDT", 288.24210641, 0.5, Coin.Exchange.Binance));
            portfolio.Add(new Coin("dusk-network", "tDUSK:USD", 1031.92729236, 1, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("ocean-protocol", "tOCEAN:USD", 151.04895717, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("canto", "CANTO_USDT", 569.04305500, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("big-data-protocol", "BDP_USDT", 1135.55438100, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("celer-network", "CELRUSDT", 4254.83785800, 0.5, Coin.Exchange.Binance));
            portfolio.Add(new Coin("arbitrum", "tARBUSD", 115.37294410, 1, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("vela-token", "VELA_USDT", 47.07204200, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("blockstack", "STXUSDT", 209.71685500, 1, Coin.Exchange.Binance));
            portfolio.Add(new Coin("joe", "JOEUSDT", 192.62475857, 0.5, Coin.Exchange.Binance));
            portfolio.Add(new Coin("fetch-ai", "tFETUSD", 197.70116570, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("oraichain-token", "ORAI_USDT", 28.01983200, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("iexec-rlc", "RLCUSDT", 43.13283800, 0.5, Coin.Exchange.Binance));
            portfolio.Add(new Coin("render-token", "RNDRUSDT", 35.83685000, 0.5, Coin.Exchange.Binance));
            portfolio.Add(new Coin("akash-network", "AKT_USDT", 72.29333400, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("alethea-artificial-liquid-intelligence-token", "ALI_USDT", 2985.33516000, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("lido-dao", "tBGBUSD", 27.61227695, 1, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("bitget-token", "tBGBUSD", 219.98778448, 1, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("alexgo", "ALEX_USDT", 741.47400000, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("astar", "ASTRUSDT", 899.20000000, 0.5, Coin.Exchange.Binance));
            portfolio.Add(new Coin("maverick-protocol", "MAVUSDT", 205.75000000, 0.5, Coin.Exchange.Binance));
            portfolio.Add(new Coin("radiant-capital", "RDNTUSDT", 221.70000000, 0.5, Coin.Exchange.Binance));
            portfolio.Add(new Coin("pendle", "PENDLEUSDT", 77.50000000, 0.5, Coin.Exchange.Binance));
            portfolio.Add(new Coin("moonwell-artemis", "WELL_USDT", 8933.99420000, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("arkadiko-protocol", "WELL_USDT", 8591.53477000, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("conic-finance", "WELL_USDT", 107.06748355, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("unibot", "WELL_USDT", 3.15638680, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("empyreal", "WELL_USDT", 3.47820368, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("rocketx", "WELL_USDT", 235.55784810, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("mute", "WELL_USDT", 468.33025218, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("winr-protocol", "WELL_USDT", 1744.05382522, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("stepn", "WELL_USDT", 664.23330000, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("injective-protocol", "WELL_USDT", 9.77520000, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("ezkalibur", "WELL_USDT", 1035.08605023, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("eclipse-fi", "WELL_USDT", 460.75485000, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("celestia", "WELL_USDT", 10.22270328, 0.5, Coin.Exchange.GateIO));            
            portfolio.Add(new Coin("bonk", "WELL_USDT", 11787000, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("honk", "WELL_USDT", 14028.9, 0.5, Coin.Exchange.GateIO));




            return portfolio;
        }


    }

}


