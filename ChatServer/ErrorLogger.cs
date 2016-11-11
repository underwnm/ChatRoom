using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class ErrorLogger : ILogger
    {
        public void WriteToConsole(string message)
        {
            Console.WriteLine("[ERROR] {0}", message);
        }
    }
}
