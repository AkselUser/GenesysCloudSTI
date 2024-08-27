using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesysCloudSTI
{
    internal class BearerToken
    {
        public static string bearerToken { get; set; }
        public static void PostToken()
        {
            string contentString = String.Format("{{\"userName\": \"{0}\",\"password\": \"{1}\"}}", System.Configuration.ConfigurationManager.AppSettings["tokenUserName"], System.Configuration.ConfigurationManager.AppSettings["tokenPassword"]);
            var content = new StringContent(contentString, Encoding.UTF8, "application/json");


            var response = HttpClient.Client.PostAsync(System.Configuration.ConfigurationManager.AppSettings["tokenUrl"], content).Result;
            var newResult = response.Content.ReadAsStringAsync();
            bearerToken = newResult.Result;
        }

    }
}
