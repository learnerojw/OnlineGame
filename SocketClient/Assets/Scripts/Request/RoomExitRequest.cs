using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExitRequest : BaseRequest
{
    private MainPack mainPack = null;
    public override void Start()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.Exit;
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
                        UIManager.GetInstance().PopPanel("RoomPanel");
                        UIManager.GetInstance().PushPanel<RoomListPanel>("RoomListPanel", (panel) =>
                        {
                            UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                            {
                                panel.ShowMessage("成功退出房间");
                            });
                        });
                        mainPack = null;
                        break;
                    }
                case ReturnCode.Fail:
                    {
                        UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                        {
                            panel.ShowMessage("退出房间失败");
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

    public void SendRequest(string roonName)
    {
        MainPack mainPack = new MainPack();
        mainPack.Requestcode = requestCode;
        mainPack.Actioncode = actionCode;
        RoomPack roomPack = new RoomPack();
        roomPack.Roomname = roonName;
        mainPack.RoompackList.Add(roomPack);
        //mainPack.Str = "666";
        base.SendRequest(mainPack);
    }
}
