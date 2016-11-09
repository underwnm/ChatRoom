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
        private TcpListener server;
        private List<Client> clients;
        private Queue<Message> messages;
        public Server()
        {
            server = new TcpListener(localAddress, port);
            clients = new List<Client>();
            messages = new Queue<Message>();
        }
        public void StartServer()
        {
            Task listen = new Task (() => ListenForClients());
            listen.Start();
        }
        private void ListenForClients()
        {
            server.Start();
            int counter = 0;

            while (true)
            {
                try
                {
                    TcpClient client = server.AcceptTcpClient();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
                counter++;
                //clients.Add(new Client(client, counter));
                Task clientTask = new Task(() =>
                {
                    HandleClient(clients[counter-1]);
                });
                clientTask.Start();
            }
        }
        private void HandleClient(Client client)
        {
            TcpClient chat = client.tcpClient;
            NetworkStream chatStream = chat.GetStream();

            byte[] message = new byte[4096];
            int byteRead;
            string data;

            ASCIIEncoding encode = new ASCIIEncoding();
            byte[] sendData = new byte[4096];
            Parallel.Invoke(() =>
            {
                if (chatStream.DataAvailable == true)
                {
                    byteRead = 1;
                    byteRead = chatStream.Read(message, 0, 4096);
                    data = Encoding.ASCII.GetString(message, 0, byteRead);
                    messages.Enqueue(new Message(data, client.id));
                    Console.WriteLine("Received: {0}", data);
                }
            },
            () =>
            {
                BroadcastMessage();
            });
        }
        private void BroadcastMessage()
        {
            foreach (Message msg in messages)
            {
                Console.WriteLine("{0}", msg.message);
            }
        }
        //private void ListenForClients()
        //{
        //    try
        //    {
        //        while (true)
        //        {
        //            int clientID = 0;
        //            Console.WriteLine("Waiting for a connection... ");
        //            TcpClient tcpClient = server.AcceptTcpClient();
        //            clients.Add(new Client(tcpClient, clientID));
        //            clientID++;
        //            Task listen = new Task(() => ReceiveAllDataFromClient(clients[clientID-1]));
        //            listen.Start();
        //            Console.WriteLine("Connected!");
        //        }
        //    }
        //    catch (SocketException e)
        //    {
        //        Console.WriteLine("SocketException: {0}", e);
        //    }
        //    finally
        //    {
        //        server.Stop();
        //    }
        //    Console.WriteLine("\nHit enter to continue...");
        //    Console.Read();
        //}
        //private void ReceiveAllDataFromClient(Client client)
        //{
        //    Byte[] bytes = new Byte[256];
        //    String data = null;
        //    NetworkStream stream = client.tcpClient.GetStream();
        //    int count;
        //    while ((count = stream.Read(bytes, 0, bytes.Length)) != 0)
        //    {
        //        data = Encoding.ASCII.GetString(bytes, 0, count);
        //        Console.WriteLine("Received: {0}", data);
        //        messages.Enqueue(new Message(data.ToString(), client.id));
        //        messages.ToArray();
        //        foreach (Message clientMessage in messages)
        //        {
        //            byte[] message = Encoding.ASCII.GetBytes(clientMessage.message);
        //            stream.Write(message, 0, message.Length);
        //            Console.WriteLine("Sent: {0}", clientMessage.message);
        //        }
        //    }
        //}
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
