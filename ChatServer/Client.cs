using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class Client : IObserver
    {
        public string username;
        public NetworkStream stream;
        public TcpClient tcpClient;
        private ASCIIEncoding encoder = new ASCIIEncoding();
        public Client(TcpClient tcpClient, NetworkStream stream)
        {
            this.tcpClient = tcpClient;
            this.stream = stream;
        }
        public void SetUsername()
        {
            SendUsernamePrompt("Enter your username");
            ReceiveUsername();
            CheckDictionary();
        }
        private void SendUsernamePrompt(string message)
        {
            byte[] writeBuffer = encoder.GetBytes(message);
            int offset = 0;
            stream.Write(writeBuffer, offset, writeBuffer.Length);
        }
        private void ReceiveUsername()
        {
            byte[] readBuffer = new byte[1024];
            int offset = 0;
            int numberOfBytesRead = stream.Read(readBuffer, offset, readBuffer.Length);
            username = encoder.GetString(readBuffer, offset, numberOfBytesRead);
        }
        private void CheckDictionary()
        {
            if(Server.clientDictionary.ContainsKey(username))
            {
                SendUsernamePrompt("Username already in use! Please pick Another...");
                ReceiveUsername();
                CheckDictionary();
            }
        }
        public void Receiving()
        {
            bool clientInChannel = true;
            while (clientInChannel)
            {
                try
                {
                    byte[] readBuffer = new byte[1024];
                    int offset = 0;
                    StringBuilder completeMessage = new StringBuilder();
                    do
                    {
                        int numberOfBytesRead = stream.Read(readBuffer, offset, readBuffer.Length);
                        completeMessage.Append(encoder.GetString(readBuffer, offset, numberOfBytesRead));
                    }
                    while (stream.DataAvailable);
                    string message = username + ": " + completeMessage.ToString();
                    Server.messages.Enqueue(new Message(message, this));
                }
                catch
                {
                    stream.Close();
                    tcpClient.Close();
                    Server.Disconnect(this);
                    clientInChannel = false;
                }
            }
        }
        public void Update(string message)
        {
            byte[] writeBuffer = new byte[1024];
            int offset = 0;
            writeBuffer = encoder.GetBytes(message);
            stream.Write(writeBuffer, offset, writeBuffer.Length);
        }
    }
}
