using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR.Client;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DateTime lastMessageReceived = DateTime.Now.AddDays(-1);

            var hubConnection = new HubConnection(string.Format("http://localhost:{0}/", args[0]));
            IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("product");

            stockTickerHubProxy.On<string>("updateProduct", Console.WriteLine);

            Console.WriteLine("Options = 's' for 'Start', 'i' for 'Interrupt', 'c' for 'Continue', 'x' for 'Stop':");
            ConsoleKeyInfo input = Console.ReadKey(true);

            while (input.KeyChar != 'x')
            {
                if (input.KeyChar == 's')
                {
                    hubConnection.Start().Wait(TimeSpan.FromSeconds(1));
                }
                else if (input.KeyChar == 'c')
                {
                    hubConnection.Start().Wait();
                    Console.WriteLine("---- Catching Up ----");
                    var results = stockTickerHubProxy.Invoke<IList<string>>("CatchUp", lastMessageReceived).Result;
                    results.ToList().ForEach(Console.WriteLine);
                    Console.WriteLine("---- Done Catching Up ----");
                }
                else if (input.KeyChar == 'i')
                {
                    lastMessageReceived = DateTime.Now;
                    hubConnection.Stop();
                }
                input = Console.ReadKey(true);
            }

            hubConnection.Stop();
        }
    }
}
