using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoomRequest : BaseRequest
{
    private MainPack mainPack = null;
    public override void Start()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.CreateRoom;
        base.Start();
    }
    private void Update()
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
                                panel.ShowMessage("创建房间成功");
                            });
                            mainPack = null;
                        });
                        break;
                    }
                case ReturnCode.Fail:
                    {
                        UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                        {
                            panel.ShowMessage("创建房间失败");
                            mainPack = null;
                        });
                        break;
                    }
                default:
                    {
                        Debug.Log("既没有成功也没有失败");
                        mainPack = null;
                        break;
                    }
            }
            
        }
    }
    public void SendRequest(string roomName,int maxNum)
    {
        MainPack pack = new MainPack();
        pack.Requestcode = requestCode;
        pack.Actioncode = actionCode;
        RoomPack roomPack = new RoomPack();
        roomPack.Roomname = roomName;
        roomPack.Maxnum = maxNum;
        pack.RoompackList.Add(roomPack);
        pack.Str = "666";
        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        base.OnResponse(pack);
        this.mainPack = pack;
    }

    
}
