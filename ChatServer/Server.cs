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
    public class Server : ISubject
    {
        private TcpListener tcpListener;
        private NetworkStream chatStream;
        public static Dictionary<string, Client> clientDictionary = new Dictionary<string, Client>();
        public static Queue<Message> messages = new Queue<Message>();      
        public void StartServer()
        {
            int port = GetPortNumber("Please enter port for server");
            string ip = FindLocalIpAddress();
            IPAddress localAddress = IPAddress.Parse(ip);
            Console.Clear();
            Console.WriteLine("Chat Server started on {0}:{1}", ip, port);
            tcpListener = new TcpListener(localAddress, port);
            Parallel.Invoke(ListenForClients, NotifyAll);
        }
        private void ListenForClients()
        {
            try
            {
                tcpListener.Start();
            }
            catch (SocketException)
            {
                Console.WriteLine("Already a server listening on this port");
                StartServer();
            }
            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                chatStream = tcpClient.GetStream();
                Task addNewUser = Task.Run(() => HandleNewClient(tcpClient));
            }
        }
        private void HandleNewClient(TcpClient tcpClient)
        {
            Client newClient = new Client(tcpClient, chatStream);
            newClient.SetUsername();
            Task startReceiving = Task.Run(() => newClient.Receiving());
            Connect(newClient);
            startReceiving.Wait();
            Disconnect(newClient);
        }
        private string FindLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach(var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
        private int GetPortNumber(string message)
        {
            Console.WriteLine(message);
            int userInput;
            int.TryParse(Console.ReadLine(), out userInput);
            if(userInput < 65535 && userInput > 0)
            {
                return userInput;
            }
            return GetPortNumber("Invalid port entered... Please Re-enter a valid port...");
        }
        public void Connect(Client client)
        {
            ILogger log = new ClientConnectLogger();
            clientDictionary.Add(client.username, client);
            string message = client.username + " connected...";
            log.WriteToConsole(client.username.ToUpper());
            messages.Enqueue(new Message(message, client));
        }
        public void Disconnect(Client client)
        {
            ILogger log = new ClientDisconnectLogger();
            clientDictionary.Remove(client.username);
            string message = client.username + " disconnected...";
            log.WriteToConsole(client.username.ToUpper());
            messages.Enqueue(new Message(message, client));
        }
        public void NotifyAll()
        {
            while (true)
            {
                if (messages.Count > 0)
                {
                    Message message = messages.Dequeue();
                    foreach (Client entry in clientDictionary.Values)
                    {
                        entry.Update(message.message);
                    }
                }
            }
        }
    }
}
