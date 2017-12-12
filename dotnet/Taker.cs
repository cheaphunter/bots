using Nethereum.Util;
using System;

namespace EhterDelta.Bots.Dontnet
{
    public class Taker : BaseBot
    {
        public Taker(EtherDeltaConfiguration config, ILogger logger = null) : base(config, logger)
        {
            var order = Service.GetBestAvailableSell();

            if (order != null)
            {
                Console.WriteLine($"Best available: Sell {order.EthAvailableVolume.ToString("N3")} @ {order.Price.ToString("N9")}");
                var desiredAmountBase = 0.001m;

                var fraction = Math.Min(desiredAmountBase / order.EthAvailableVolumeBase, 1);
                try
                {
                    var uc = new UnitConversion();
                    var amount = order.AmountGet.Value * uc.ToWei(fraction);
                    Service.TakeOrder(order, amount).Wait();
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                    }
                    else
                    {
                        Console.WriteLine(ex.Message);
                    }
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("No Available order");
            }

            Console.WriteLine();
        }
    }
}