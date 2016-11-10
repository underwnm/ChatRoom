﻿using System;
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
        private int port = 8888;
        private string server = "127.0.0.1";
        public void StartClient()
        {
            client = new TcpClient(server, port);
            Console.WriteLine("CONNECTED TO {0}:{1}", server, port);
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
    }
}
