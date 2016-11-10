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
        private List<User> users = new List<User>();
        private Dictionary<string, User> dictionary = new Dictionary<string, User>();
        public static Queue<Message> messages = new Queue<Message>();
        public void StartServer()
        {
            int port = 8888;
            string ip = "127.0.0.1";
            IPAddress localAddress = IPAddress.Parse(ip);
            Console.WriteLine("Chat Server started on {0}:{1}", ip, port);
            tcpListener = new TcpListener(localAddress, port);
            Parallel.Invoke(ListenForClients, SendToAll);
        }
        private void ListenForClients()
        {
            tcpListener.Start();
            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                chatStream = tcpClient.GetStream();
                Task addNewUser = Task.Run(() => AddNewUser());
            }
        }
        private void AddNewUser()
        {
            User newUser = new User(chatStream);
            Task setUsername = Task.Run(() => newUser.SetUsername(dictionary));
            setUsername.Wait();
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
                        try
                        {
                            writeBuffer = encoder.GetBytes(message.message);
                            users[i].stream.Write(writeBuffer, offset, writeBuffer.Length);
                            Console.WriteLine("Sent to {0}: {1}", users[i].username, message.message);
                        }
                        catch
                        {
                            users[i].stream.Close();
                            dictionary.Remove(users[i].username);
                            users.RemoveAt(i);
                        }
                    }
                }
            }
        }
    }
}
