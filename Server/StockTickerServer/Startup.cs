using System.Configuration;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using StockTickerServer;

[assembly: OwinStartup(typeof (Startup), "Configuration")]

namespace StockTickerServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.UseSqlServer(
                ConfigurationManager.ConnectionStrings["SignalR"].ConnectionString);

            app.MapSignalR();
        }
    }
}