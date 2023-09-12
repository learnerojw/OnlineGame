using SocketGameServer.Controller;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MySql.Data.MySqlClient;
using SocketGameProtocol;

namespace SocketGameServer.Servers
{
    class Server
    {
        private Socket socket;
        private byte[] buffer = new byte[1024];
        private List<Client> clientList = new List<Client>();
        private List<Room> roomList = new List<Room>();

        private ControllerManager controllerManager;
        private UDPServer udpServer;
        public Server(int port)
        {
            controllerManager = new ControllerManager(this);
            udpServer = new UDPServer(6667,this,controllerManager);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
            socket.Listen(10);
            
            StartAccept();
            Console.Read();
        }


        void StartAccept()
        {
            socket.BeginAccept(AcceptCallback, null);
        }
        void AcceptCallback(IAsyncResult iar)
        {
            //注意：这里的client是与对方客户端连接的socket，用于服务端向客户端发送消息或接受客户端发送过来的消息
            //这句代码的意思是建立与对方客户端的socket连接，可以通过该socket向客户端socket发送和接受消息
            Socket client = socket.EndAccept(iar);
            clientList.Add(new Client(client,this,udpServer));
            //StartReceive(client);
            StartAccept();
        }
        
        public void HandleRequest(MainPack pack,Client client)
        {
            controllerManager.HandleRequest(pack, client);
        }

        //移除客户端
        public void RemoveClient(Client client)
        {
            if(clientList.Contains(client))
            {
                clientList.Remove(client);
            }
        }
        //移除房间
        public void RemoveRoom(Room room)
        {
            if(roomList.Contains(room))
            {
                roomList.Remove(room);
            }
        }


        public ReturnCode CreateRoom(Client client,MainPack pack)
        {
            try
            {
                //创建房间，添加进房间列表
                Room room = new Room(this,client, pack.RoompackList[0]);
                roomList.Add(room);
                
                //获得当前房间的玩家列表，用于更新客户端房间中的玩家信息
                foreach (PlayerPack playerPack in room.GetRoomPlayerList())
                {
                    pack.PlayerPackList.Add(playerPack);
                }
                //Console.WriteLine(pack.PlayerPackList.Count);
                return ReturnCode.Succeed;
                
            }
            catch
            {                
                return ReturnCode.Fail;
            }
        }

        public MainPack FindRoom()
        {
            MainPack pack = new MainPack();
            pack.Actioncode = ActionCode.FindRoom;
            
            try
            {
                if(roomList.Count==0)
                {
                    pack.Returncode = ReturnCode.NotRoom;
                }
                else
                {
                    foreach (Room room in roomList)
                    {
                        pack.RoompackList.Add(room.GetRoomInfo);
                    }
                    pack.Returncode = ReturnCode.Succeed;
                }
            }
            catch
            {
                pack.Returncode = ReturnCode.Fail;
            }

            return pack;
        }


        public MainPack JoinRoom(Client client,MainPack mainPack)
        {            
            foreach(Room room in roomList)
            {
                if (room.GetRoomInfo.Roomname == mainPack.RoompackList[0].Roomname)
                {
                    //可以加入房间
                    if(room.GetRoomInfo.State==0)
                    {
                        
                        //房间加入玩家
                        room.AddPlayer(client);
                        mainPack.RoompackList[0] = room.GetRoomInfo;
                        //更新包中的玩家列表
                        foreach (PlayerPack playerPack in room.GetRoomPlayerList())
                        {
                            mainPack.PlayerPackList.Add(playerPack);
                        }
                        mainPack.Returncode = ReturnCode.Succeed;                    
                        return mainPack;
                    }
                    else
                    {
                        mainPack.Returncode = ReturnCode.Fail;
                        return mainPack;
                    }
                }
            }
            mainPack.Returncode = ReturnCode.NotRoom;
            return mainPack;
        }

        public MainPack ExitRoom(Client client, MainPack mainPack)
        {
            if(client.PlayerInfo.room==null)
            {
                mainPack.Returncode = ReturnCode.Fail;
                return mainPack;
            }
            try
            {
                foreach (Room room in roomList)
                {
                    if (room.GetRoomInfo.Roomname == mainPack.RoompackList[0].Roomname)
                    {
                        room.Exit(client);
                        break;                        
                    }
                }
                mainPack.Returncode = ReturnCode.Succeed;
                return mainPack;
            }
            catch
            {
                mainPack.Returncode = ReturnCode.Fail;
                return mainPack;
            }
        }


        public MainPack Chat(Client client,MainPack mainPack)
        {
            if(client.PlayerInfo.room==null)
            {
                mainPack.Returncode = ReturnCode.Fail;
                return mainPack;
            }
            //将客户端发送的聊天信息进行加工
            mainPack.ChatText = client.PlayerInfo.PlayerPack.PlayerName + " : " + mainPack.ChatText;
            mainPack.Returncode = ReturnCode.Succeed;
            //给房间里其他客户端发送聊天消息
            client.PlayerInfo.room.Chat(client,mainPack);            
            return mainPack;
        }

        public MainPack StartGame(Client client, MainPack mainPack)
        {
            
            client.PlayerInfo.room.StartGame(client, mainPack);

            //这里返回mainpack是告诉客户端是否成功开始游戏，若是房主就成功，不是房主则是失败。
            return mainPack;
        }

        public Client ClientFromUserName(string userName)
        {
            foreach(Client client in clientList)
            {
                if(client.PlayerInfo.PlayerPack.PlayerName==userName)
                {
                    return client;
                }
            }
            return null;
        }
    }
}
