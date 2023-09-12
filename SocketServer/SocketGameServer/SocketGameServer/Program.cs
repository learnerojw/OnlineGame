using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using SocketGameServer.Servers;

namespace SocketGameServer
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(6666);
            Console.Read();
        }
        
    }
}
