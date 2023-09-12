using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoomRequest : BaseRequest
{
    private MainPack mainPack = null;
    
    public override void Start()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.JoinRoom;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(mainPack!=null)
        {
            switch (mainPack.Returncode)
            {
                case ReturnCode.Succeed:
                    {
                        UIManager.GetInstance().PopPanel("RoomListPanel");
                        UIManager.GetInstance().PushPanel<RoomPanel>("RoomPanel", (panel) =>
                        {
                            //更新房间名字
                            panel.roomName.text = mainPack.RoompackList[0].Roomname;
                            //创建和加入时都会收到服务器传输的含有房间玩家列表的包
                            panel.UpdatePlayerList(mainPack);
                            UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                            {
                                panel.ShowMessage("加入房间成功");
                            });
                            mainPack = null;
                        });
                        
                        break;
                    }
                case ReturnCode.Fail:
                    {
                        UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                        {
                            panel.ShowMessage("加入房间失败");
                        });
                        mainPack = null;
                        break;
                    }
                case ReturnCode.NotRoom:
                    {
                        UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                        {
                            panel.ShowMessage("房主已解散该房间,请重新刷新");
                        });
                        mainPack = null;
                        break;
                    }
                default:
                    {
                        mainPack = null;
                        break;
                    }
            }


        }
    }

    public override void OnResponse(MainPack pack)
    {
        base.OnResponse(pack);
        this.mainPack = pack;
    }

    public void SendRequest(string roomname)
    {
        MainPack mainPack = new MainPack();
        mainPack.Requestcode = requestCode;
        mainPack.Actioncode = actionCode;
        RoomPack roomPack = new RoomPack();
        roomPack.Roomname = roomname;
        mainPack.RoompackList.Add(roomPack);
        mainPack.Str = "666";
        base.SendRequest(mainPack);
    }
}
