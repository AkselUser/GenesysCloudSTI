using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Net.Http;

namespace GenesysCloudSTI
{
    public class HttpClient
    {
        public static System.Net.Http.HttpClient Client { get; set; }
    }
}
