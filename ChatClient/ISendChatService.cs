using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    public interface ISendChatService
    {
        void SendMessage(string message, string receiver);
        void Start(string name);
        void Stop(string name);
    }
}
