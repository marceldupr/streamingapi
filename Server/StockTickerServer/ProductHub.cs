using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using Dapper;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Messaging;

namespace StockTickerServer
{
    [HubName("product")]
    public class ProductHub : Hub
    {
        private const string MessageQuery = "select * from SignalR.Messages_0 where InsertedOn > @lastMessageDate";
        private static int _messageId = 1;
        private static readonly object Mutex = new object();
        private Timer _timer;

        public ProductHub()
        {
            _timer = new Timer(state =>
            {
                Clients.All.updateProduct(string.Format("ID {0}: Product has been updated", _messageId));
                lock (Mutex)
                {
                    _messageId++;
                }
            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(2));
        }

        public IEnumerable<string> CatchUp(DateTime lastMessageDate)
        {
            IEnumerable<string> messages;
            using (var connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["SignalR"].ConnectionString))
            {
                var queryResults = connection.Query(MessageQuery, new {lastMessageDate});
                messages = Transform(queryResults);
            }
            
            return messages;
        }

        private static IEnumerable<string> Transform(IEnumerable<dynamic> queryResults)
        {
            return queryResults.Select(x => ScaleoutMessage.FromBytes(x.Payload))
                .Select(x => x.Messages[0])
                .Cast<Message>()
                .Select(x => System.Text.Encoding.UTF8.GetString(x.Value.Array))
                .Select(x => Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(x).A)
                .Select(x => x != null ? x[0].Value : string.Empty)
                .Cast<string>()
                .ToArray();
        }
    }
}