using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ChatServer
{
    public class Server
    {
        private static int port = 8888;
        private static IPAddress localAddress = IPAddress.Parse("127.0.0.1");
        private TcpListener server = new TcpListener(localAddress, port);
        private Thread listenThread;
        TcpClient client;
        List<TcpClient> clientList = new List<TcpClient>();
        private void StartServer()
        {
            listenThread = new Thread(new ThreadStart(ListenForClients));
            listenThread.Start();
        }
        private void ListenForClients()
        {
            while (true)
            {
                Console.WriteLine("Waiting for a connection... ");
                client = server.AcceptTcpClient();
                clientList.Add(client);
                Console.WriteLine("Connected!");
                ReceiveAllDataFromClient(client);
                client.Close();
            }
        }
        private void ReceiveAllDataFromClient(TcpClient client)
        {
            Byte[] bytes = new Byte[256];
            String data = null;
            NetworkStream stream = client.GetStream();
            int count;
            while ((count = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = Encoding.ASCII.GetString(bytes, 0, count);
                Console.WriteLine("Received: {0}", data);
                data = data.ToUpper();
                byte[] message = Encoding.ASCII.GetBytes(data);
                stream.Write(message, 0, message.Length);
                Console.WriteLine("Sent: {0}", data);
            }
        }
        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
    }
}
