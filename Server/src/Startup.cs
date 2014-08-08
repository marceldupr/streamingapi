using System.Configuration;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using Server;

[assembly: OwinStartup(typeof (Startup), "Configuration")]

namespace Server
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