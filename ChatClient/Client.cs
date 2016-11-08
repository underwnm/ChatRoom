using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace ChatClient
{
    public class Client
    {
        TcpClient newClient;
        private static int port = 8888;
        private static string localAddress = IPAddress.Parse("127.0.0.1").ToString();
        public Client(string server, string message)
        {

        }
        private void StartClient()
        {
            newClient = new TcpClient(localAddress, port);
        }
        private void GetServerInformation()
        {

        }
    }
}
