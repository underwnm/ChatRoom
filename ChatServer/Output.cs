using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class Output : ILogger
    {
        public void Error(string message)
        {
            throw new NotImplementedException();
        }

        public void Info(string message)
        {
            Console.WriteLine(message);
        }
    }
}
