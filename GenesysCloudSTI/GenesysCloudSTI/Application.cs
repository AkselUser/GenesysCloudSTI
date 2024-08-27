using GenesysCloudSTI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesysCloudSTI
{
    internal class Application
    {
        public static void RunApplication()
        {
            Log.Logger.Info("Starting Program .. .");

            AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", @"..\App.config");

            Configure.RunConfig();
            RunningWebsocket.CollectWebsocketOutput().Wait();
        }
    }
}