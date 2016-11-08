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
        TcpClient newClient;
        private static int port = 8888;
        private static string server = IPAddress.Parse("127.0.0.1").ToString();
        string message = "TESTING HELLO WORLD";
        public void StartClient()
        {
            newClient = new TcpClient(server, port);
            Connect();
        }
        private void Connect()
        {
            try
            {
                Byte[] data = Encoding.ASCII.GetBytes(message);
                NetworkStream stream = newClient.GetStream();
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent: {0}", message);
                data = new Byte[256];
                String responseData = String.Empty;
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);
                stream.Close();
                newClient.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }
    }
}
