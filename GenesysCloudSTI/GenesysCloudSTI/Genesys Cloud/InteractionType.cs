using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesysCloudSTI
{
    internal class InteractionType
    {
        public static string GetConversationType(ResponseSchema.Participant customer)
        {
            if (customer.calls != null)
            {
                return System.Configuration.ConfigurationManager.AppSettings["eventIdCall"];
            }
            else
            {
                if (customer.emails != null)
                {
                    return System.Configuration.ConfigurationManager.AppSettings["eventIdEmail"];
                }
                else
                {
                    if (customer.messages != null)
                    {
                        return System.Configuration.ConfigurationManager.AppSettings["eventIdChat"];
                    }
                }
            }
            return null;
        }
    }
}
