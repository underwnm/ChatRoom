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
        private Thread listenThread;
        private TcpListener tcpListener;
        private ASCIIEncoding encoder = new ASCIIEncoding();
        //public Dictionary<DateTime, string> users = new Dictionary<DateTime, string>();
        private List<Client> clients = new List<Client>();
        public static Queue<Message> messages = new Queue<Message>();

        public void StartServer()
        {
            int port = 8888;
            IPAddress localAddress = IPAddress.Parse("127.0.0.1");
            Console.WriteLine("Starting server on port: {0}", port);
            tcpListener = new TcpListener(localAddress, port);
            Parallel.Invoke(ListenForClients, SendToAll);
        }
        private void ListenForClients()
        {
            tcpListener.Start();
            while (true)
            {
                Console.WriteLine("Waiting for connection...");
                TcpClient client = tcpListener.AcceptTcpClient();                
                Client newClient = GetUserName(client);
                clients.Add(newClient);
                Thread newClientThread = new Thread(new ThreadStart(newClient.Receiving));
                newClientThread.Start();
            }
        }
        private Client GetUserName(TcpClient client)
        {
            NetworkStream chatStream = client.GetStream();
            string message = "Enter your username";
            byte[] data = encoder.GetBytes(message);
            Console.WriteLine("Send: {0}", message);
            chatStream.Write(data, 0, data.Length);
            byte[] buffer = new byte[4096];
            int byteRead = chatStream.Read(buffer, 0, buffer.Length);
            string userName = encoder.GetString(buffer, 0, byteRead);
            Client newClient = new Client(chatStream, userName);
            return newClient;
        }
        public void SendToAll()
        {
            while (true)
            {
                try
                {
                    if (messages.Count > 0)
                    {
                        Message message = messages.Dequeue();
                        foreach (Client client in clients)
                        {
                            byte[] data = encoder.GetBytes(message.message);
                            client.stream.Write(data, 0, data.Length);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Nothing in message list");
                }
            }
        }
    }
}
