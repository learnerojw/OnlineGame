using System;
using System.Collections.Generic;
using System.Text;
using SocketGameProtocol;
using SocketGameServer.Servers;

namespace SocketGameServer.Controller
{
    class UserController:BaseController
    {
        public UserController()
        {
            requestCode = RequestCode.User;
        }


        public MainPack Register(Server server,Client client,MainPack pack)
        {
            if(client.GetUserData.Register(pack))
            {
                pack.Returncode = ReturnCode.Succeed;
            }
            else
            {
                pack.Returncode = ReturnCode.Fail;
            }
            return pack;
        }

        public MainPack Logon(Server server, Client client, MainPack pack)
        {
            if(client.GetUserData.Logon(pack))
            {
                pack.Returncode = ReturnCode.Succeed;
                client.PlayerInfo.SetInfo(pack.PlayerPackList[0]);
            }
            else
            {
                pack.Returncode = ReturnCode.Fail;
            }
            return pack;
        }
    }
}
