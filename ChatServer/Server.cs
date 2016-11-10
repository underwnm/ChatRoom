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
        private List<User> clients = new List<User>();
        public static Queue<Message> messages = new Queue<Message>();

        public void StartServer()
        {
            int port = 8888;
            string ip = "127.0.0.1";
            IPAddress localAddress = IPAddress.Parse(ip);
            Console.WriteLine("Starting server on {0}:{1}", ip, port);
            tcpListener = new TcpListener(localAddress, port);
            Parallel.Invoke(ListenForClients, SendToAll);
        }
        private void ListenForClients()
        {
            tcpListener.Start();
            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();                
                User newClient = GetUserName(tcpClient);
                clients.Add(newClient);
                Thread newClientThread = new Thread(new ThreadStart(newClient.Receiving));
                newClientThread.Start();
            }
        }
        private User GetUserName(TcpClient tcpClient)
        {
            chatStream = tcpClient.GetStream();
            Send("Enter your username");
            string userName = Receive();
            User newClient = new User(chatStream, userName);
            return newClient;
        }
        private void Send(string message)
        {
            int offset = 0;
            byte[] writeBuffer = encoder.GetBytes(message);
            Console.WriteLine("Send: {0}", message);
            chatStream.Write(writeBuffer, offset, writeBuffer.Length);
        }
        private string Receive()
        {
            int offset = 0;
            byte[] readBuffer = new byte[1024];
            int numberOfBytesRead = chatStream.Read(readBuffer, offset, readBuffer.Length);
            string userName = encoder.GetString(readBuffer, offset, numberOfBytesRead);
            return userName;
        }
        public void SendToAll()
        {
            while (true)
            {
                if (messages.Count > 0)
                {
                    byte[] writeBuffer = new byte[1024];
                    int offset = 0;
                    Message message = messages.Dequeue();
                    foreach (User client in clients)
                    {
                        writeBuffer = encoder.GetBytes(message.message);
                        client.stream.Write(writeBuffer, offset, writeBuffer.Length);
                        Console.WriteLine("Sent to {0}: {1}", client.username, message.message);
                    }
                }
            }
        }
    }
}
