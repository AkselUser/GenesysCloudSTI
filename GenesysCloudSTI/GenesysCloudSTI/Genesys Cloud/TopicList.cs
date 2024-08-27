using NLog;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Model;
using System.Collections.Specialized;
using System.Configuration;

namespace GenesysCloudSTI

{
    internal static class TopicList
    {
        public static List<ChannelTopic> SetUpTopicList()
        {
            NameValueCollection queueListSection = (NameValueCollection)System.Configuration.ConfigurationManager.GetSection("Queues");

            List<ChannelTopic> topicList = new List<ChannelTopic>();
            foreach (var queue in queueListSection)
            {
                var topicQueueArray = queueListSection[queue.ToString()].Split(";");
                foreach (var topicQueue in topicQueueArray)
                {
                    topicList.Add(new ChannelTopic()
                    {
                        Id = $"v2.routing.queues.{topicQueue}.conversations"
                    });
                }
            }
            return (topicList);    
        }
    }
}
