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
                            //���·�������
                            panel.roomName.text = mainPack.RoompackList[0].Roomname;
                            //�����ͼ���ʱ�����յ�����������ĺ��з�������б�İ�
                            panel.UpdatePlayerList(mainPack);
                            UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                            {
                                panel.ShowMessage("��������ɹ�");
                            });
                            mainPack = null;
                        });
                        break;
                    }
                case ReturnCode.Fail:
                    {
                        UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                        {
                            panel.ShowMessage("��������ʧ��");
                            mainPack = null;
                        });
                        break;
                    }
                default:
                    {
                        Debug.Log("��û�гɹ�Ҳû��ʧ��");
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
