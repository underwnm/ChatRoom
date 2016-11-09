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
        TcpClient client;
        NetworkStream clientStream;
        ASCIIEncoding encoder = new ASCIIEncoding();
        byte[] buffer = new byte[4096];
        private static int port = 8888;
        private static string server = "127.0.0.1";
        public void StartClient()
        {
            client = new TcpClient(server, port);
            Console.WriteLine("CONNECTED TO SERVER!");
            Parallel.Invoke(() =>
            {
                ClientReceive();
            },
            () =>
            {
                SendMessage();
            });
            ClientReceive();
            //Connect();
        }
        //private void Connect()
        //{
        //    try
        //    {
        //        while (true)
        //        {
        //            string message = GetUserInput();
        //            Byte[] data = Encoding.ASCII.GetBytes(message);
        //            NetworkStream stream = client.GetStream();
        //            stream.Write(data, 0, data.Length);
        //            Console.WriteLine("Sent: {0}", message);
        //            data = new Byte[256];
        //            String responseData = String.Empty;
        //            Int32 bytes = stream.Read(data, 0, data.Length);
        //            responseData = Encoding.ASCII.GetString(data, 0, bytes);
        //            Console.WriteLine("Received: {0}", responseData);
        //        }
        //        //stream.Close();
        //        //newClient.Close();
        //    }
        //    catch (ArgumentNullException e)
        //    {
        //        Console.WriteLine("ArgumentNullException: {0}", e);
        //    }
        //    catch (SocketException e)
        //    {
        //        Console.WriteLine("SocketException: {0}", e);
        //    }
        //    Console.WriteLine("\n Press Enter to continue...");
        //    Console.Read();
        //} 
        private string EnterMessage()
        {
            Console.WriteLine("Type Message: ");
            string userInput = Console.ReadLine();
            return userInput;
        }
        private void SendMessage()
        {
            string message = EnterMessage();
            Task send = new Task(() =>
            {
                clientStream = client.GetStream();
                buffer = encoder.GetBytes(message);
                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();
            });
            send.Start();
        }
        private void ClientReceive()
        {
            clientStream = client.GetStream();
            Task receive = new Task(() =>
            {
                int i;
                string data;

                while (true)
                {
                    i = 0;
                    i = clientStream.Read(buffer, 0, buffer.Length);
                    if (i == 0)
                    {
                        break;
                    }
                    data = encoder.GetString(buffer, 0, i);
                    Console.WriteLine("Received: {0}", data);
                }
            });
            receive.Start();
        }
    }
}
