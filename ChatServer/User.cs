using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class User
    {
        public string username;
        public NetworkStream stream;
        private ASCIIEncoding encoder = new ASCIIEncoding();
        public User(NetworkStream stream)
        {
            this.stream = stream;
        }
        public void Receiving()
        {
            bool userLeftChat = false;
            while (!userLeftChat)
            {
                if (stream.CanRead)
                {
                    byte[] readBuffer = new byte[1024];
                    int offset = 0;
                    StringBuilder completeMessage = new StringBuilder();
                    try
                    {
                        do
                        {
                            int numberOfBytesRead = stream.Read(readBuffer, offset, readBuffer.Length);
                            completeMessage.Append(encoder.GetString(readBuffer, offset, numberOfBytesRead));
                        }
                        while (stream.DataAvailable);
                        string message = username + ": " + completeMessage.ToString();
                        Server.messages.Enqueue(new Message(message));
                        Console.WriteLine("Received {0}'s message: {0}", username, completeMessage.ToString());
                    }
                    catch
                    {
                        string disconnected = username + " disconnected...";
                        Server.messages.Enqueue(new Message(disconnected));
                        Console.WriteLine(disconnected);
                        userLeftChat = true;
                    }
                }
                else
                {
                    Console.WriteLine("Sorry. The server cannot read from {0} NetworkStream", username);
                }
            }
        }
        public void SetUsername()
        {
            SendUsernamePrompt("Enter your username");
            ReceiveUsername();
            string connected = username + " connected...";
            Server.messages.Enqueue(new Message(connected));
        }
        private void SendUsernamePrompt(string message)
        {
            byte[] writeBuffer = encoder.GetBytes(message);
            int offset = 0;
            Console.WriteLine("Send: {0}", message);
            stream.Write(writeBuffer, offset, writeBuffer.Length);
        }
        private void ReceiveUsername()
        {
            byte[] readBuffer = new byte[1024];
            int offset = 0;
            int numberOfBytesRead = stream.Read(readBuffer, offset, readBuffer.Length);
            username = encoder.GetString(readBuffer, offset, numberOfBytesRead);
        }
    }
}
