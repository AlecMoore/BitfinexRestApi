using Bitfinex.Net.Clients;
using Bitfinex.Net.Enums;
using Bitfinex.Net.Objects;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Logging;

namespace BotCore
{
    public class program
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
            setUp();

            var symbolData = await bitfinexClient.SpotApi.ExchangeData.GetSymbolsAsync();
            var tickerData = await bitfinexClient.SpotApi.ExchangeData.GetTickersAsync();
            var ticker = await bitfinexClient.SpotApi.ExchangeData.GetTickerAsync("tBTCUSD");
            var accountData = await bitfinexClient.SpotApi.Account.GetBalancesAsync();

            var symbolData1 = await bitfinexClient.SpotApi.Trading.PlaceOrderAsync(
                "tBTCUSD",
                OrderSide.Buy,
                OrderType.ExchangeMarket,
                0.00006m,
                0);
            var wank = "wank";
        }
    }
}
