using SocketGameProtocol;
using SocketGameServer.Servers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

namespace SocketGameServer.Controller
{
    class RoomController: BaseController
    {
        public RoomController()
        {
            requestCode = RequestCode.Room;
        }

        public MainPack CreateRoom(Server server,Client client,MainPack pack)
        {
            pack.Returncode=server.CreateRoom(client, pack);
            return pack;
        }

        public MainPack FindRoom(Server server, Client client, MainPack pack)
        {
             return server.FindRoom();
        }

        public MainPack JoinRoom(Server server, Client client, MainPack pack)
        {
            return server.JoinRoom(client, pack);
        }

        public MainPack Exit(Server server, Client client, MainPack pack)
        {
            return server.ExitRoom(client, pack);
        }

        public MainPack Chat(Server server, Client client, MainPack pack)
        {
            return server.Chat(client, pack);
        }

        public MainPack StartGame(Server server, Client client, MainPack pack)
        {
            return server.StartGame(client, pack);
        }
    }
}
