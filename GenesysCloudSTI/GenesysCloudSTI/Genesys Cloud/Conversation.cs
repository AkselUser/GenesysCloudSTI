using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesysCloudSTI
{
    internal class Conversation
    {
        internal bool answered { get; set; }
        internal string conversationId { get; set; }
        internal bool terminated { get; set; }
    }
}
