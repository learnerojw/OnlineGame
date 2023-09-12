using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using SocketGameServer.Tool;
using SocketGameServer.DAO;
using SocketGameProtocol;
using System.Net;

namespace SocketGameServer.Servers
{
    class Client
    {
        private Socket socket;
        private Message message;
        private UserData userData;
        private Server server;
        private PlayerInfo playerInfo;
        private EndPoint remoteEP;//远程连接主机IP,用于UDP传输
        public UDPServer udpServer;
        public UserData GetUserData
        {
            get
            {
                return userData;
            }
        }

        public PlayerInfo PlayerInfo
        {
            get
            {
                return playerInfo;
            }
        }

        public EndPoint RemoveEP
        {
            get
            {
                return remoteEP;
            }
            set
            {
                remoteEP = value;
            }
        }
        public Client(Socket socket,Server server,UDPServer udpServer)
        {
            message = new Message();
            userData = new UserData();
            playerInfo = new PlayerInfo();
            this.socket = socket;
            this.server = server;
            this.udpServer = udpServer;
            StartReceive();
        }

        void StartReceive()
        {
            socket.BeginReceive(message.Buffer,message.StartIndex,message.Remsize,SocketFlags.None, ReceiveCallBack, null);
        }
        void ReceiveCallBack(IAsyncResult iar)
        {
            try
            {
                if (socket == null || socket.Connected == false) return;
                int len = socket.EndReceive(iar);
                if(len==0)
                {
                    return;
                }
                message.ReadBuffer(len,HandleRequest);
                Console.WriteLine("收到客户端的消息");
                StartReceive();
            }
            catch
            {
                Close();
            }
        }
        //TCP发送
        public void Send(MainPack pack)
        {
            //将pack包转换为字节流，同时使用包头+包体的方式进行发送
            socket.Send(Message.PackData(pack));
        }
        //UDP发送
        public void SendTo(MainPack pack)
        {
            if (remoteEP == null) return;
            udpServer.SendTo(pack,RemoveEP);
        }
        void HandleRequest(MainPack pack)
        {
            server.HandleRequest(pack, this);
        }

        private void Close()
        {
            if(playerInfo.room!=null)
            {
                playerInfo.room.Exit(this);
            }
            server.RemoveClient(this);
            socket.Close();
            userData.GetMysqlcon.Close();
        }
    }
}
