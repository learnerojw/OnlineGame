using SocketGameProtocol;
using SocketGameServer.Servers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocketGameServer.Controller
{
    class GameController:BaseController
    {
        public GameController()
        {
            requestCode = RequestCode.Game;
        }

        public MainPack GameingExit(Server server, Client client, MainPack pack)
        {
            client.PlayerInfo.room.GameingExit(client, pack);
            return pack;
        }

        
        public MainPack UpdatePos(Server server, Client client, MainPack pack)
        {
            pack.Returncode = ReturnCode.Succeed;
            //使用UDP转发
            client.PlayerInfo.room.BroadCastTo(client, pack);
            //记录当前客户端的位置
            if(client.PlayerInfo.PlayerPack.PosPack==null)
            {
                client.PlayerInfo.PlayerPack.PosPack = new PosPack();
            }
            client.PlayerInfo.PlayerPack.PosPack.PosX = pack.PlayerPackList[0].PosPack.PosX;
            client.PlayerInfo.PlayerPack.PosPack.PosY = pack.PlayerPackList[0].PosPack.PosY;
            return null;
        }

        public MainPack Fire(Server server, Client client, MainPack pack)
        {
            pack.Returncode = ReturnCode.Succeed;
            //使用UDP转发
            client.PlayerInfo.room.BroadCastTo(client, pack);
            return null;
        }

        public MainPack Damage(Server server, Client client, MainPack pack)
        {
            client.PlayerInfo.room.Damage(client, pack);
            return null;
        }
    }
}
