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
        private TcpListener tcpListener;
        private NetworkStream chatStream;
        private ASCIIEncoding encoder = new ASCIIEncoding();
        public static Dictionary<string, User> dictionary = new Dictionary<string, User>();
        public static List<User> users = new List<User>();
        public static Queue<Message> messages = new Queue<Message>();
        public void StartServer()
        {
            int port = GetPortNumber("Please enter port for server");
            string ip = GetLocalIpAddress();
            IPAddress localAddress = IPAddress.Parse(ip);
            Console.Clear();
            Console.WriteLine("Chat Server started on {0}:{1}", ip, port);
            tcpListener = new TcpListener(localAddress, port);
            Parallel.Invoke(ListenForClients, SendToAll);
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
                Task addNewUser = Task.Run(() => AddNewUser(tcpClient));
            }
        }
        private void AddNewUser(TcpClient tcpClient)
        {
            User newUser = new User(tcpClient, chatStream);
            newUser.SetUsername(dictionary);
            Task startReceiving = Task.Run(() => newUser.Receiving());
            users.Add(newUser);
        }
        private void SendToAll()
        {
            while (true)
            {
                if (messages.Count > 0)
                {
                    byte[] writeBuffer = new byte[1024];
                    int offset = 0;
                    Message message = messages.Dequeue();
                    for (int i = 0; i < users.Count; i++)
                    {
                        writeBuffer = encoder.GetBytes(message.message);
                        users[i].stream.Write(writeBuffer, offset, writeBuffer.Length);
                        Console.WriteLine("Sent to {0}: {1}", users[i].username, message.message);
                    }
                }
            }
        }
        private string GetLocalIpAddress()
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
    }
}
