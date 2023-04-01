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

namespace PositionCalculator
{
    class Program
    {
        private static BitfinexClient bitfinexClient;
        private static SpotApi gateSpotApi;

        private static async void setUp()
        {
            bitfinexClient = new BitfinexClient(new BitfinexClientOptions()
            {
                //ApiCredentials = new ApiCredentials("cVEc61LYLYUtXFxpSf668FuST5m5WfXRkx2M7xgbq4H", "bBqKbdGFT8YVRSvTNBVMPaGaPaBJuq5ZEfbacU97BSH"),
                ApiCredentials = new ApiCredentials("mlYORC0XslzL0ROLqCYBpYWz9HX8TuJOoXgebQbP2u9", "1nTTBGVfjJdx885oVPsRIsP6Aapi8XagpXjOy2FCT7E"),
                LogLevel = LogLevel.Trace
                //RequestTimeout = TimeSpan.FromSeconds(60)
            });

            Configuration gateConfig = new Configuration
            {
                // Setting basePath is optional. It defaults to https://api.gateio.ws/api/v4
                //BasePath = _runConfig.HostUsed,
                ApiV4Key = "a2cb8f421a727e2be020b17942e7e30f",
                ApiV4Secret = "465492dc945bbbee59f6a69e1413b4e231d4b645cb1c937c872b1c1c38685399"
            };
            gateSpotApi = new SpotApi(gateConfig);
        }

        //a2cb8f421a727e2be020b17942e7e30f
        //465492dc945bbbee59f6a69e1413b4e231d4b645cb1c937c872b1c1c38685399
        public static async Task Main(string[] args)
        {
            double maximumValue = 10000.0;
            double buyTotal = 0.0;
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
                coin.buyAmountUsd = ((coin.targetPercentage - coin.currentPercentage) * maximumValue / 100) / 6;
                coin.buyAmountCrypto = coin.buyAmountUsd / coin.price;
                buyTotal += coin.buyAmountUsd;
                Console.WriteLine("Values: {0} {1} {2} {3} {4} {5}", coin.names.GeckoName, coin.units, coin.price, coin.currentPercentage, coin.targetPercentage, coin.buyAmountUsd);


                if (coin.buyAmountCrypto > 0)
                {
                    if (coin.exchange== Coin.Exchange.Bitfinex)
                    {
                        var symbolData1 = await bitfinexClient.SpotApi.Trading.PlaceOrderAsync(
                            coin.names.ApiCoinName,
                            OrderSide.Buy,
                            OrderType.ExchangeMarket,
                            (decimal)coin.buyAmountCrypto,
                            0);
                    }

                    if (coin.exchange == Coin.Exchange.GateIO) 
                    {
                        try
                        {
                            List<Ticker> tickers = gateSpotApi.ListTickers(coin.names.ApiCoinName);
                            string lastPrice = tickers[0].Last;

                            //Order order = new Order(currencyPair: coin.names.ApiCoinName, amount: coin.buyAmountCrypto.ToString(CultureInfo.InvariantCulture), type: Order.TypeEnum.Market)
                            //{
                            //    Account = Order.AccountEnum.Spot,
                            //    AutoBorrow = false,
                            //    Side = Order.SideEnum.Buy,
                            //    TimeInForce = Order.TimeInForceEnum.Ioc
                            //};

                            Order order = new Order(currencyPair: coin.names.ApiCoinName, amount: coin.buyAmountCrypto.ToString(CultureInfo.InvariantCulture), price: lastPrice)
                            {
                                Account = Order.AccountEnum.Spot,
                                Side = Order.SideEnum.Buy,
                            };

                            var sds = await gateSpotApi.CreateOrderAsync(order);
                        } catch (Exception ex)
                        { 
                            Console.WriteLine($"{ex.Message}");
                        }

                    }
                }
            }
            Console.WriteLine("Total: " + buyTotal);
        }

        static List<Coin> GetPortfolio()
        {
            List<Coin> portfolio = new List<Coin>();
            //portfolio.Add(new Coin("ethereum", "tETHUSD", 2.62531911, 50, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("matic-network", "tMATIC:USD", 99.81646549, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("chainlink", "tLINK:USD", 12.49951265, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("curve-dao-token", "tCRVUSD", 17.21200000, 0.5, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("frax-share", "FXS_USDT", 2.13319000, 0.5, Coin.Exchange.GateIO));
            //portfolio.Add(new Coin("convex-finance", "CVX_USDT", 2.85445700, 0.5, Coin.Exchange.GateIO));
            //portfolio.Add(new Coin("0x", "tZRXUSD", 72.50541334, 0.5, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("helium", "HNT_USDT", 106.36157969, 2, Coin.Exchange.GateIO));
            //portfolio.Add(new Coin("oasis-network", "tROSE:USD", 551.78160003, 0.5, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("cosmos", "tATOUSD", 5.60173701, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("avalanche-2", "tAVAX:USD", 6.86521277, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("polkadot", "tDOTUSD", 5.22539752, 0.5, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("near", "tNEAR:USD", 10.17600000, 0.5, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("solana", "tSOLUSD", 7.12126158, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("fantom", "tFTMUSD", 395.04315638, 2, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("thorchain", "RUNE_USDT", 15.54999200, 0.5, Coin.Exchange.GateIO));
            //portfolio.Add(new Coin("algorand", "tALGUSD", 119.88505985, 0.5, Coin.Exchange.Bitfinex));
            //portfolio.Add(new Coin("binancecoin", "BNB_USDT", 0.41718226, 2, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("tron", "tTRXUSD", 280.38795181, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("uniswap", "tUNIUSD", 9.67637925, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("aave", "tAAVE:USD", 0.74037471, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("zcash", "tZECUSD", 1.80898000, 2, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("dogecoin", "tDOGE:USD", 231.20570685, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("gmx", "GMX_USDT", 0, 1, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("the-graph", "tGRTUSD", 148.87752022, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("aptos", "tAPTUSD", 3.79413842, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("havven", "tSNXUSD", 3.72843452, 1, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("optimism", "OP_USDT", 5.15452900, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("dydx", "DYDX_USDT", 5.37634200, 1, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("radix", "tXRDUSD", 612.98062286, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("singularitynet", "AGIX_USDT", 15.48178600, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("dusk-network", "tDUSK:USD", 38.65274651, 1, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("ocean-protocol", "tOCEAN:USD", 18.93937237, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("canto", "CANTO_USDT", 20.36261500, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("big-data-protocol", "BDP_USDT", 25.56229800, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("celer-network", "CELR_USDT", 404.89346000, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("arbitrum", "tARBUSD", 0, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("vela-token", "VELA_USDT", 0, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("blockstack", "STX_USDT", 0, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("joe", "JOE_USDT", 0, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("fetch-ai", "tFETUSD", 0, 0.5, Coin.Exchange.Bitfinex));
            portfolio.Add(new Coin("oraichain-token", "ORAI_USDT", 0, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("iexec-rlc", "RLC_USDT", 0, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("render-token", "RNDR_USDT", 0, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("akash-network", "AKT_USDT", 0, 0.5, Coin.Exchange.GateIO));
            portfolio.Add(new Coin("alethea-artificial-liquid-intelligence-token", "ALI_USDT", 0, 0.5, Coin.Exchange.GateIO));

            return portfolio;
        }


    }

}


