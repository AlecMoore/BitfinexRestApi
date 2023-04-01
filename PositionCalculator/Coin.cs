using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PositionCalculator
{
    class Coin
    {
        public Names names { get; set; }
        public double units { get; set; }
        public double currentPercentage { get; set; }
        public double targetPercentage { get; set; }
        public double price { get; set; }
        public double value { get; set; }
        public double buyAmountUsd { get; set; }
        public double buyAmountCrypto { get; set; }
        public Exchange exchange { get; set; }

        public Coin(string geckoName, string ApiCoinName, double units, double targetPercentage, Exchange exchange)
        {
            this.names = new Names(geckoName, ApiCoinName);
            this.units = units;
            this.targetPercentage = targetPercentage;
            this.exchange = exchange;
        }

        public enum Exchange
        {
            Bitfinex,
            GateIO,
            Coinbase,
            Kraken,
            CakeDefi,
            CoinCorner,
            Nexo
        }

        public class Names
        {
            public string GeckoName { get; set; }
            public string ApiCoinName { get; set; }

            public Names(string geckoName, string apiCoinName)
            {
                GeckoName = geckoName;
                ApiCoinName = apiCoinName;
            }
        }
    }
}