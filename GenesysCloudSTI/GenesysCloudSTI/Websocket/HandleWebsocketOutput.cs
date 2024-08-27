using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GenesysCloudSTI
{
    internal class HandleWebsocketOutput
    {
        internal static void GetTerminatedConversations(string rawOutput)
        {
            if (!string.IsNullOrEmpty(rawOutput))
            {
                var responseSchema = JsonSerializer.Deserialize<ResponseSchema.Rootobject>(rawOutput);
                //Check Json is not heartbeat
                if (responseSchema?.eventBody.participants != null)
                {
                    var participants = responseSchema.eventBody.participants;
                    List<string> quarantinedConversations = new List<string>();
                    if (participants.Where(p => p.purpose == "agent").Any() && participants.Where(p => p.purpose == "customer").Any())
                    {
                        var conversationId = responseSchema.eventBody.id;
                        var customer = participants.Where(p => p.purpose == "customer").First();
                        JObject jObj = JObject.Parse(rawOutput);
                        switch (InteractionType.GetConversationType(customer))
                        {
                            case "105":
                                {
                                    if (
                                        participants.First(p => p.purpose == "customer").calls.First().state == "terminated" &&
                            !quarantinedConversations.Contains(conversationId) &&
                            participants.First(p => p.purpose == "agent").connectedTime.Year != 0001 &&
                            participants.First(p=>p.purpose == "agent").wrapup == null && participants.First(p => p.purpose == "customer").calls.First().disconnectType != "endpoint"
                            )
                                    {
                                        quarantinedConversations.Add(conversationId);
                                        Task.Run(() =>
                                        {
                                            Thread.Sleep(10000);
                                            quarantinedConversations.RemoveAll(x=> x == conversationId);
                                        });
                                        PostRequest.PostRequestToSTI(participants, "105", conversationId);
                                    }
                                    break;
                                }
                            case "106":
                                {
                                    if (
                                        participants.Where(p => p.purpose == "customer").First().emails.First().state == "disconnected" &&
                                        !quarantinedConversations.Contains(conversationId) &&
                                        participants.First(p => p.purpose == "agent").wrapup == null
                                        )
                                    {
                                        participants.First(p => p.purpose == "customer").attributes.email = jObj["eventBody"]["participants"][0]["attributes"]["personEpost"].ToString();
                                        participants.First(p => p.purpose == "customer").attributes.phone = jObj["eventBody"]["participants"][0]["attributes"]["personPhone"].ToString();
                                        PostRequest.PostRequestToSTI(participants, "106", conversationId);
                                        Task.Run(() =>
                                        {
                                            quarantinedConversations.Add(conversationId);
                                            Thread.Sleep(10000);
                                            quarantinedConversations.Remove(conversationId);
                                        });
                                    }
                                    break;
                                }
                            case "107":
                                {
                                    if (
                                        !quarantinedConversations.Contains(conversationId) && 
                                        participants.Where(p => p.purpose == "agent").First().messages.First().state == "disconnected" && participants.Where(p => p.purpose == "customer").First().messages.First().state == "disconnected" &&
                                        participants.First(p => p.purpose == "agent").wrapup == null &&
                                        (participants.First(x => x.purpose == "agent").endTime - participants.First(x => x.purpose == "agent").connectedTime).Seconds > 20
                                        )
                                    {
                                        participants.First(p => p.purpose == "customer").attributes.phone = jObj["eventBody"]["participants"][0]["attributes"]["phone"].ToString();
                                        participants.First(p => p.purpose == "customer").attributes.email = jObj["eventBody"]["participants"][0]["attributes"]["email"].ToString();
                                        PostRequest.PostRequestToSTI(participants, "107", conversationId);
                                        Task.Run(() =>
                                        {
                                            quarantinedConversations.Add(conversationId);
                                            Thread.Sleep(10000);
                                            quarantinedConversations.Remove(conversationId);
                                        });
                                    }
                                    break;
                                }
                            default:
                                {

                                    break;
                                }
                        }

                    }
                }
            }
        }
    }
}
