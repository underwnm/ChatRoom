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
                string server = GetServerIp("Enter the IP of the chat server...");
                int port = GetPortNumber("Enter the port of the chat server");
                client = new TcpClient(server, port);
                Console.Clear();
                Console.WriteLine("CONNECTED TO {0}:{1}", server, port);
            }
            catch
            {
                Console.Clear();
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
                try
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
                catch
                {
                    DisplayServerCrashed();
                }
            }
        }
        private void DisplayServerCrashed()
        {
            Console.WriteLine("Server Crashed...");
            Console.WriteLine("Hit any key to restart program");
            Console.ReadKey();
            Console.Clear();
            StartClient();
        }
        private void ClearLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop -1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
        private string GetServerIp(string message)
        {
            Console.WriteLine(message);
            string userInput = EnterMessage();
            return userInput;
        }
        private int GetPortNumber(string message)
        {
            Console.WriteLine(message);
            int userInput;
            int.TryParse(Console.ReadLine(), out userInput);
            if (userInput < 65535 && userInput > 0)
            {
                return userInput;
            }
            return GetPortNumber("Invalid port entered... Please Re-enter a valid port...");
        }
    }
}
