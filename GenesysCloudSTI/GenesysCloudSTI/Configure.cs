using System.Configuration;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Client;
using PureCloudPlatform.Client.V2.Extensions;
using PureCloudPlatform.Client.V2.Model;

namespace GenesysCloudSTI
{
    public class Configure
    {

        public static void RunConfig()
        {
            HttpClient.Client = new System.Net.Http.HttpClient();

            SetEnvironmentAndTokens();
            BearerToken.PostToken();
            // Renew Genesys Cloud OAuth token and STI bearer token every day
            Task.Run(() =>
            {
                while (true)
                {
                    
                    Thread.Sleep(86400 * 1000);
                    SetEnvironmentAndTokens();
                    BearerToken.PostToken();
                }
            });
            API.InitializeApis();
            NotificationChannel.Channel = API.notificationsApi.PostNotificationsChannels();

            API.notificationsApi.PutNotificationsChannelSubscriptions(NotificationChannel.Channel.Id, TopicList.SetUpTopicList());

        }

        public static void SetEnvironmentAndTokens()
        {
            SetEnvironment();
            SetToken();
        }

        private static void SetToken()
        {
            var clientId = System.Configuration.ConfigurationManager.AppSettings["clientId"];
            var clientSecret = System.Configuration.ConfigurationManager.AppSettings["clientSecret"];

            // Set access token as described in GC documentation
            AuthTokenInfo accessTokenInfo = PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.PostToken(
                clientId,
                clientSecret);
            PureCloudPlatform.Client.V2.Client.Configuration.Default.AccessToken = accessTokenInfo.AccessToken;
            Log.Logger.Info("New OAuth token retrieved", accessTokenInfo.AccessToken);
        }

        private static void SetEnvironment()
        {
            // Set environment
            PureCloudRegionHosts region = PureCloudRegionHosts.eu_west_1;
            PureCloudPlatform.Client.V2.Client.Configuration.Default.ApiClient.setBasePath(region);
            Log.Logger.Info("Environment set: {0}", region);
        }
    }
}
