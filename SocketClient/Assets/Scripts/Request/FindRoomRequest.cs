using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindRoomRequest : BaseRequest
{
    private MainPack mainPack = null;
    private RoomListPanel roomListPanel;
    public override void Start()
    {
        roomListPanel = GetComponent<RoomListPanel>();
        requestCode = RequestCode.Room;
        actionCode = ActionCode.FindRoom;
        base.Start();
    }

    private void Update()
    {
        if (mainPack != null)
        {
            switch (mainPack.Returncode)
            {

                case ReturnCode.Succeed:
                    {
                        UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                        {
                            panel.ShowMessage("查询到"+mainPack.RoompackList.Count+"个房间");
                            UpdateRoomList(mainPack);
                            mainPack = null;
                        });
                        break;
                    }
                case ReturnCode.Fail:
                    {
                        UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                        {
                            panel.ShowMessage("查询房间出错");
                        });
                        mainPack = null;
                        break;
                    }
                case ReturnCode.NotRoom:
                    {
                        UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                        {
                            panel.ShowMessage("当前没有房间");
                        });
                        UpdateRoomList(mainPack);
                        mainPack = null;
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
    public void SendRequest()
    {
        MainPack pack = new MainPack();
        pack.Requestcode = requestCode;
        pack.Actioncode = actionCode;
        pack.Str = "r";
        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        base.OnResponse(pack);
        this.mainPack = pack;
    }

    private void UpdateRoomList(MainPack Pack)
    {
        for(int i=0;i<roomListPanel.roomListTr.childCount;i++)
        {
            Destroy(roomListPanel.roomListTr.GetChild(i).gameObject);
        }

        foreach(RoomPack Room in Pack.RoompackList)
        {
            ResMgr.GetInstance().LoadAsync<GameObject>("UI/RoomItem", (obj) =>
            {
                obj.transform.SetParent(roomListPanel.roomListTr);
                obj.GetComponent<RoomItem>().SetRoomInfo(Room);
            });
        }
    }
}
