using SocketGameProtocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocketGameServer.Servers
{
    class PlayerInfo
    {
        //用于保存客户端的玩家信息，并打包发送用于更新房间里的玩家信息
        private PlayerPack playerPack;
        //该客户端此时所加入的房间
        public Room room;
        public PlayerPack PlayerPack
        {
            get
            {
                return playerPack;
            }
        }
        public void SetInfo(PlayerPack playerPack)
        {
            this.playerPack = playerPack;
        }
    }
}
