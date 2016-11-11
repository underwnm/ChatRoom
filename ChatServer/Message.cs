using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class Message
    {
        public string message;
        public Client sender;
        public Message(string message, Client sender)
        {
            this.message = message;
            this.sender = sender;
        }
    }
}
