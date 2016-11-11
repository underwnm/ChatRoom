using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class ClientConnectLogger : ILogger
    {
        public void WriteToConsole(string message)
        {
            Console.WriteLine("[{0} CONNECTED]", message);
        }
    }
}
