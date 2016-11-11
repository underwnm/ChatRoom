using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class MessageLogger : ILogger
    {
        public void WriteToConsole(string message)
        {
            Console.WriteLine("[MESSAGE] {0}", message);
        }
    }
}
