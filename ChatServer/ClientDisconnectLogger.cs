using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class ClientDisconnectLogger : ILogger
    {
        public void WriteToConsole(string message)
        {
            Console.WriteLine("[{0} DISCONNECTED]", message);
        }
    }
}
