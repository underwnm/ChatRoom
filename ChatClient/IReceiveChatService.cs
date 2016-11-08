using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
    public interface IReceiveChatService
    {
        void ReceiveMessage(string message, string sender);
        void SendNames(List<string> names);
    }
}
