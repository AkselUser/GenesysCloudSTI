using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GenesysCloudSTI
{
    internal class RunningWebsocket
    {
        internal static async Task CollectWebsocketOutput()
        {
            ClientWebSocket Client = new ClientWebSocket();
            Uri uri = new Uri(NotificationChannel.Channel.ConnectUri);
            await Client.ConnectAsync(uri, CancellationToken.None);
            Log.Logger.Info("Websocket started");
            using (var client = Client)
            {
                // While loop runs forever
                string rawOutput = "";
                ArraySegment<byte> bytesReceived = new ArraySegment<byte>(new byte[1024]);
                WebSocketReceiveResult result = null;
                while (true)
                {
                    try
                    {
                        while (client.State == WebSocketState.Open)
                        {
                            do
                            {
                                result = await client.ReceiveAsync(bytesReceived, CancellationToken.None).ConfigureAwait(false);

                                if (result.Count > 0)
                                {
                                    rawOutput += Encoding.UTF8.GetString(bytesReceived.Array, 0, result.Count);
                                }
                                else
                                {
                                    Console.WriteLine("Over");
                                }
                            } while (!result.EndOfMessage);
                            
                            HandleWebsocketOutput.GetTerminatedConversations(rawOutput);
                            
                            rawOutput = "";
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex); }
                    Log.Logger.Info("Websocket loop ended");
                    break;
                }
                Application.RunApplication();
            }
        }
    }
}
