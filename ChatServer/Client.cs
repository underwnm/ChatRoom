using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class Client
    {
        public int id;
        public TcpClient tcpClient;
        public Client(TcpClient tcpClient, int id)
        {
            this.tcpClient = tcpClient;
            this.id = id;
        }
    }
}
