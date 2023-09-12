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
                            //���·�������
                            panel.roomName.text = mainPack.RoompackList[0].Roomname;
                            //�����ͼ���ʱ�����յ�����������ĺ��з�������б�İ�
                            panel.UpdatePlayerList(mainPack);
                            UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                            {
                                panel.ShowMessage("���뷿��ɹ�");
                            });
                            mainPack = null;
                        });
                        
                        break;
                    }
                case ReturnCode.Fail:
                    {
                        UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                        {
                            panel.ShowMessage("���뷿��ʧ��");
                        });
                        mainPack = null;
                        break;
                    }
                case ReturnCode.NotRoom:
                    {
                        UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                        {
                            panel.ShowMessage("�����ѽ�ɢ�÷���,������ˢ��");
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
