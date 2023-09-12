using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using SocketGameServer.Controller;
using System.Threading;
using SocketGameProtocol;
using SocketGameServer.Tool;

namespace SocketGameServer.Servers
{
    class UDPServer
    {
        Socket udpSocket;
        IPEndPoint bindEP;//本地监听IP
        EndPoint remoteEP;//远程IP

        Server server;

        ControllerManager controllerManager;

        Byte[] buffer = new byte[1024];//消息缓存

        Thread receiveThread;//接受线程

        public UDPServer(int port,Server server,ControllerManager controllerManager)
        {
            this.server = server;
            this.controllerManager = controllerManager;
            udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            bindEP = new IPEndPoint(IPAddress.Any, port);//本机IP
            remoteEP = (EndPoint)bindEP;//远程IP赋值
            udpSocket.Bind(bindEP);//socket绑定本机IP
            receiveThread = new Thread(ReceiveMsg);//实例化线程
            receiveThread.Start();//开启线程，接受消息
            Console.WriteLine("UDP服务已启动");
        }

        ~UDPServer()
        {
            if(receiveThread!=null)
            {
                receiveThread.Abort();//如果线程不为空，终止线程
                receiveThread = null;
            }
        }

        public void ReceiveMsg()
        {
            while (true)
            {
                int len = udpSocket.ReceiveFrom(buffer, ref remoteEP);
                MainPack pack = (MainPack)MainPack.Descriptor.Parser.ParseFrom(buffer, 0, len);
                HandleRequest(pack, remoteEP);
            }

        }
        public void HandleRequest(MainPack pack,EndPoint ipEndPoint)
        {
            Client client = server.ClientFromUserName(pack.UserName);
            if (client == null) return;
            
            if(client.RemoveEP==null)
            {
                client.RemoveEP = ipEndPoint;
            }
            controllerManager.HandleRequest(pack, client, true);
        }


        public void SendTo(MainPack pack,EndPoint IEP)
        {
            Byte[] buff = Message.PackDataUDP(pack);
            udpSocket.SendTo(buff, buff.Length, SocketFlags.None, IEP);
        }
    }
}
