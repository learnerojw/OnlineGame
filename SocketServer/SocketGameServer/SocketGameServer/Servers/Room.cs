using SocketGameProtocol;
using System;
using System.Collections.Generic;
using System.Text;
using Google.Protobuf.Collections;
using System.Threading;
using Org.BouncyCastle.Asn1.Cms;
using System.Numerics;

namespace SocketGameServer.Servers
{
    class Room
    {
        private RoomPack roomInfo;

        private List<Client> clientList = new List<Client>();

        private Server server;

        
        public RoomPack GetRoomInfo
        {
            get
            {
                roomInfo.Curnum = clientList.Count;
                return roomInfo;
            }
        }
        public Room(Server server,Client client,RoomPack roomPack)
        {
            this.server = server;
            roomInfo = roomPack;
            clientList.Add(client);
            roomInfo.Curnum = clientList.Count;
            //设置房主的所属房间
            client.PlayerInfo.room = this;
            //设置房间状态
            SetRoomState();
        }

        public RepeatedField<PlayerPack> GetRoomPlayerList()
        {
            //给列表添加房间里的玩家信息(playerpack)
            RepeatedField<PlayerPack> pack = new RepeatedField<PlayerPack>();
            foreach(Client client in clientList)
            {
                pack.Add(client.PlayerInfo.PlayerPack);
            }
            return pack;
        }
        //TCP转发
        public void BroadCast(Client client,MainPack pack)
        {
            foreach(Client c in clientList)
            {
                if(c==client)
                {
                    continue;
                }
                c.Send(pack);
            }
        }
        //UDP转发
        public void BroadCastTo(Client client, MainPack pack)
        {
            foreach(Client c in clientList)
            {
                if (c == client) continue;
                c.SendTo(pack);
            }
        }

        public void AddPlayer(Client client)
        {
            clientList.Add(client);
            roomInfo.Curnum = clientList.Count;
            SetRoomState();
            //更新客户端当前所在房间
            client.PlayerInfo.room = this;
            //new一个新的pack,设置为获得玩家列表的请求
            MainPack mainPack = new MainPack();
            mainPack.Actioncode = ActionCode.GetPlayerList;
            foreach (PlayerPack playerPack in GetRoomPlayerList())
            {
                mainPack.PlayerPackList.Add(playerPack);
            }
            //给房间里其他客户端发送更新玩家列表的请求
            BroadCast(client, mainPack);
        }

        public void Exit(Client client)
        {
            if (!clientList.Contains(client)) return;
            MainPack mainPack = new MainPack();
            //判断该客户端是否是该房间的房主
            if (client == clientList[0])
            {
                client.PlayerInfo.room = null;
                //房间移除该客户端
                clientList.Remove(client);
                //服务器移除该房间
                server.RemoveRoom(this);

                //设置房间状态 
                SetRoomState();

                mainPack.Actioncode = ActionCode.Exit;
                mainPack.Returncode = ReturnCode.Succeed;
                //给房间里的其他客户端发送离开请求
                BroadCast(client, mainPack);
                return;
            }

            //以下是房间普通玩家离开的代码逻辑
            clientList.Remove(client);
            client.PlayerInfo.room = null;

            //设置房间状态            
            SetRoomState();
            //给房间里其他客户端发送更新玩家列表的请求

            mainPack.Actioncode = ActionCode.GetPlayerList;
            foreach (PlayerPack playerPack in GetRoomPlayerList())
            {
                mainPack.PlayerPackList.Add(playerPack);
            }
            BroadCast(client, mainPack);
        }


        public void Chat(Client client,MainPack mainPack)
        {
            BroadCast(client, mainPack);
        }

        public void StartGame(Client client,MainPack mainPack)
        {
            //判断该客户端是否是他所在房间的房主
            if (client != clientList[0])
            {
                mainPack.Returncode = ReturnCode.Fail;
                return;
            }
            //在一个线程中向房间里所有客户端 发送倒计时 和 倒计时结束后加入游戏的请求
            Thread countDown = new Thread(Time);
            countDown.Start();
            mainPack.Returncode = ReturnCode.Succeed;
        }

        private void Time()
        {
            MainPack pack = new MainPack();
            pack.Actioncode = ActionCode.Chat;
            pack.Returncode = ReturnCode.Succeed;
            pack.ChatText = "房主已开始游戏...";
            //先给房间里的所有客户端发送聊天
            BroadCast(null, pack);
            Thread.Sleep(1000);
            //问题：为什么这里不加sleep，不能发送5
            //给房间里所有客户端发送倒计时聊天
            for (int i=5;i>=1;i--)
            {
                pack.ChatText = i.ToString();
                BroadCast(null, pack);
                Thread.Sleep(1000);
            }
            //给房间里所有客户端发送 加入游戏(进入游戏场景) 的要求
            pack.Actioncode = ActionCode.JoinGame;
            foreach(Client client in clientList)
            {
                client.PlayerInfo.PlayerPack.Hp = 100;
                pack.PlayerPackList.Add(client.PlayerInfo.PlayerPack);
            }             
            BroadCast(null, pack);            
        }


        public void GameingExit(Client client,MainPack pack)
        {
            MainPack mainPack = new MainPack();
            if (client == clientList[0])
            {
                //房主退出游戏
                mainPack.Actioncode = ActionCode.GameingExit;
                mainPack.Returncode = ReturnCode.Succeed;
                BroadCast(client, mainPack);
            }
            else
            {
                //游戏中其他玩家退出
                mainPack.Actioncode = ActionCode.GameingOtherExit;
                mainPack.PlayerPackList.Add(client.PlayerInfo.PlayerPack);
                mainPack.Returncode = ReturnCode.Succeed;
                BroadCast(client, mainPack);
            }
            pack.Returncode = ReturnCode.Succeed;
        }

        public void Damage(Client client,MainPack mainPack)
        {
            foreach(Client c in clientList)
            {
                if (c.PlayerInfo.PlayerPack.PlayerName == mainPack.PlayerPackList[0].PlayerName)
                {
                    PosPack playerPos = c.PlayerInfo.PlayerPack.PosPack;
                    BulletPack bulletPos = mainPack.BulletPack;
                    double distance = Math.Sqrt(Math.Pow(playerPos.PosX - bulletPos.PosX, 2) + Math.Pow(playerPos.PosY - bulletPos.PosY, 2));
                    if(distance<1f)
                    {
                        c.PlayerInfo.PlayerPack.Hp -= 10;
                        mainPack.PlayerPackList[0].Hp = c.PlayerInfo.PlayerPack.Hp;
                        mainPack.Returncode = ReturnCode.Succeed;
                        BroadCast(null, mainPack);
                    }
                    else
                    {
                        mainPack.Returncode = ReturnCode.Fail;
                        BroadCast(null, mainPack);
                    }
                    break;
                }
            }
        }
        private void SetRoomState()
        {
            if (GetRoomInfo.Curnum < GetRoomInfo.Maxnum)
            {
                GetRoomInfo.State = 0;
            }
            else
            {
                GetRoomInfo.State = 1;
            }
        }
    }
}
