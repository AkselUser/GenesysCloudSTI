using System.Net.Http.Headers;
using System.Text;

namespace GenesysCloudSTI
{
    internal class PostRequest
    {
        //HttpContent
        private static HttpContent? CreateRequestToSTI(ResponseSchema.Participant[] participants, string conversationId, string eventId)
        {
            var Kötid = (participants.First(x => x.purpose == "agent").connectedTime - participants.First(x => x.purpose == "customer").connectedTime).Seconds.ToString();
            var CallTime = (participants.First(x => x.purpose == "agent").endTime - participants.First(x => x.purpose == "agent").connectedTime).Seconds.ToString();
            var TelephoneMobile = "";
            var PersonId = participants.First(x => x.purpose == "customer").attributes.personId;
            var respondentAddress = "";
            if ( eventId == "105")
            {
                TelephoneMobile = participants.First(x => x.purpose == "customer").address.Replace("tel:", "");
                if (participants.First(x => x.purpose == "customer").address != null)
                {
                    respondentAddress = TelephoneMobile;
                } else
                {
                    respondentAddress = "";
                }
            }
            if (eventId == "106")
            {
                if (participants.First(x => x.purpose == "customer").attributes.email != null)
                {
                    respondentAddress = participants.First(x => x.purpose == "customer").attributes.email.ToString();
                } else
                {
                    respondentAddress = "";
                }
                Kötid = "";
                CallTime = "";
                TelephoneMobile = participants.First(x => x.purpose == "customer").attributes.phone;
            }
            if (eventId == "107")
            {
                if (participants.First(x => x.purpose == "customer").attributes.phone != null)
                {
                    respondentAddress = participants.First(x => x.purpose == "customer").attributes.phone;
                } else
                {
                    respondentAddress = participants.First(x => x.purpose == "customer").attributes.email;
                }
                TelephoneMobile = participants.First(x => x.purpose == "customer").attributes.phone;
            }
            string externalReference = $"{{" +
                $"Workgroup: \'{API.routingApi.GetRoutingQueue(participants.First(x => x.purpose == "customer").queueId).Name}\', " +
                $"Kötid: \'{Kötid}\', " +
                $"CallID: \'{conversationId}\', " +
                $"CallTime: \'{CallTime}\', " +
                $"PersonId: \'{PersonId}\', " +
                $"TelephoneMobile: \'{TelephoneMobile}\'}}";

            string contentString = $"[{{" +
                $"\"OrganizationExternalId\": \"{participants.First(x => x.purpose == "agent").userId}\", " +
                $"\"respondentAddress\": \"{respondentAddress}\"," +
                $"\"ExternalReference\": \"{externalReference}\"}} ]";

            Log.Logger.Info(contentString);
            var content = new StringContent(contentString, Encoding.UTF8, "application/json");

            return content;
        }
        public static void PostRequestToSTI(ResponseSchema.Participant[] participants, string eventId, string conversationId)
        {
            HttpClient.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken.bearerToken);
            
            Log.Logger.Info("Sending content to STI: ");
            var response = HttpClient.Client.PostAsync(
            System.Configuration.ConfigurationManager.AppSettings["respondentsUrl"] + eventId,
            CreateRequestToSTI(participants, conversationId, eventId)
            ).Result;
            var newResult = response.Content.ReadAsStringAsync();
            var result = newResult.Result;
            Log.Logger.Info("Result: " + result.ToString());

            //CreateRequestToSTI(participants, conversationId, eventId);
        }
    }
}
