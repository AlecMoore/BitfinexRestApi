//using BitfinexApi;
//using BotCore;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace FunctionApp
//{
//    public class Function1
//    {
//        private static Fng btcFng;
//        private static BalancesResponse balances;
//        private static int price;
//        private static decimal cycleAmount;
//        private static int buyThreshold = 20;
//        private static int sellThreshold = 80;
//        private static OrderSide cycleType;
//        private static BitfinexApiV1 api;


//        private static void setUp()
//        {
//            api = new BitfinexApiV1("", "");
//        }

//        private static async Task fngBotAsync()
//        {

//            await GetDataAsync();
//            await Logic();

//        }

//        private static double ConvertUSDToBTC(double usdAmount)
//        {
//            return usdAmount / price;
//        }

//        private static async Task Logic()
//        {

//            if (btcFng.value <= buyThreshold)
//            {
//                cycleType = OrderSide.Buy;
//                cycleAmount = (decimal)ConvertUSDToBTC((double)balances.totalAvailableUSD / 4.0); //need to convert to BTC amount
//                                                                                 //await Cycle();
//            }
//            else if (btcFng.value >= sellThreshold)
//            {
//                cycleType = OrderSide.Sell;
//                cycleAmount = balances.totalAvailableUSD / (decimal) 10.0;
//            }
//            else
//            {
//                return;
//            }

//            var call = api.ExecuteOrder(OrderSymbol.BTCUSD, cycleAmount, price, OrderExchange.Bitfinex, cycleType, OrderType.MarginMarket);
//        }

//        private static async Task GetDataAsync()
//        {
//            //Console.WriteLine("GetData");
//            btcFng = await FngApiUtil.ReadFNG();
//            balances = api.GetBalances();
//            price = await CoinGeckoUtil.readPrice();
//        }
//        /*("0 0 6 * * *")*/

//        [FunctionName("Function1")]
//        public async Task Run([TimerTrigger("0 * * * * *")] TimerInfo myTimer, ILogger log)
//        {
//            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
//            setUp();
//            log.LogInformation("setup done");
//            await fngBotAsync();
//            log.LogInformation("run done");
//        }
//    }
//}
