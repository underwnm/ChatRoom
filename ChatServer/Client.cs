using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class Client
    {
        public string username;
        public NetworkStream stream;
        public Client(NetworkStream stream, string username)
        {
            this.stream = stream;
            this.username = username;
        }
        public void Receiving()
        {
            byte[] buffer = new byte[4096];
            int byteRead;
            while ((byteRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                string data = Encoding.ASCII.GetString(buffer, 0, byteRead);
                Server.messages.Enqueue(new Message(data));
                Console.WriteLine("Received: {0}", data);
            }
        }
    }
}
