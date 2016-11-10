using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace ChatClient
{
    public class Client
    {
        private TcpClient client;
        private NetworkStream clientStream;
        private ASCIIEncoding encoder = new ASCIIEncoding();
        public void StartClient()
        {
            try
            {
                string server = GetServerInfo("Enter the IP of the chat server...");
                int port = Convert.ToInt32(GetServerInfo("Enter the port of the chat server"));
                client = new TcpClient(server, port);
                Console.WriteLine("CONNECTING TO {0}:{1}", server, port);
            }
            catch
            {
                Console.WriteLine("Could not find server. Please try again...");
                StartClient();
            }
            Parallel.Invoke(Receiving, SendMessage);
        }
        private string EnterMessage()
        {
            string userInput = Console.ReadLine();
            return userInput;
        }
        private void SendMessage()
        {
            while (true)
            {
                string message = EnterMessage();
                ClearLine();
                if (message != "")
                {
                    byte[] writeBuffer = new byte[1024];
                    int offset = 0;
                    clientStream = client.GetStream();
                    writeBuffer = encoder.GetBytes(message);
                    clientStream.Write(writeBuffer, offset, writeBuffer.Length);
                }
            }
        }
        private void Receiving()
        {
            clientStream = client.GetStream();
            while (true)
            {
                if (clientStream.CanRead)
                {
                    byte[] readBuffer = new byte[1024];
                    int offset = 0;
                    int numberOfBytesRead = clientStream.Read(readBuffer, offset, readBuffer.Length);
                    string message = Encoding.ASCII.GetString(readBuffer, offset, numberOfBytesRead);
                    Console.WriteLine(message);
                }
                else
                {
                    Console.WriteLine("Sorry. You cannot read from this NetworkStream.");
                }
            }
        }
        private void ClearLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop -1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
        private string GetServerInfo(string message)
        {
            Console.WriteLine(message);
            string userInput = EnterMessage();
            return userInput;
        }
    }
}
