using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Mimeo.Blocks.Network
{
    public class NetworkUtility
    {
        public static List<string> IPAddresses()
        {
            var server = Dns.GetHostName();
            IPHostEntry heServer = Dns.GetHostEntry(server);
            return heServer.AddressList.Select(x => x.ToString()).ToList();
        }
    }
}

