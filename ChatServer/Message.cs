using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class Message
    {
        public string message;
        public int sender;
        public Message(string message, int sender)
        {
            this.message = message;
            this.sender = sender;
        }
    }
}
