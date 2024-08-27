using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GenesysCloudSTI
{
    internal class WebsocketClient
    {
        internal static ClientWebSocket Client { get; set; }
        internal async static Task StartWebsocket()
        {
            Client = new ClientWebSocket();
            Uri uri = new Uri(NotificationChannel.Channel.ConnectUri);
            await Client.ConnectAsync(uri, CancellationToken.None);
            Log.Logger.Info("Websocket started");
        }
    }
}
