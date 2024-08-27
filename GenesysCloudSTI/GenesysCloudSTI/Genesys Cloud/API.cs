using PureCloudPlatform.Client.V2.Api;

namespace GenesysCloudSTI
{
    internal class API
    {
        internal static OutboundApi outboundApi { get; set; }
        internal static NotificationsApi notificationsApi { get; set; }
        internal static AnalyticsApi analyticsApi { get; set; }
        internal static RoutingApi routingApi { get; set; }

        internal static void InitializeApis()
        {
            outboundApi = new OutboundApi();
            notificationsApi = new NotificationsApi();
            analyticsApi = new AnalyticsApi();
            routingApi = new RoutingApi();
        }
    }
}
