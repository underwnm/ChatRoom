using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public interface ISubject
    {
        void Connect(Client client);
        void Disconnect(Client client);
        void NotifyAll();
    }
}
