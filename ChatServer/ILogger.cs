using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public interface ILogger
    {
        void Info(string message);
        void Error(string message);
    }
}
