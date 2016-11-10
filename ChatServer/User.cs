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
        public User(NetworkStream stream, string username)
        {
            this.stream = stream;
            this.username = username;
        }
        public void Receiving()
        {
            while (true)
            {
                if (stream.CanRead)
                {
                    byte[] readBuffer = new byte[1024];
                    StringBuilder completeMessage = new StringBuilder();
                    int numberOfBytesRead;
                    int offset = 0;
                    do
                    {
                        numberOfBytesRead = stream.Read(readBuffer, offset, readBuffer.Length);
                        completeMessage.Append(Encoding.ASCII.GetString(readBuffer, offset, numberOfBytesRead));
                    }
                    while (stream.DataAvailable);
                    string message = username + ": " + completeMessage.ToString();
                    Server.messages.Enqueue(new Message(message));
                    Console.WriteLine("Received {0}'s message: {0}", username, completeMessage.ToString());
                }
                else
                {
                    Console.WriteLine("Sorry. The server cannot read from {0} NetworkStream", username);
                }
            }
        }
    }
}
