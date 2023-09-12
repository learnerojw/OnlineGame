using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameRequest : BaseRequest
{
    private MainPack mainPack = null;
    public override void Start()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.StartGame;
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
                        UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                        {
                            panel.ShowMessage("�㿪ʼ����Ϸ!");
                        });
                        break;
                    }
                case ReturnCode.Fail:
                    {
                        UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                        {
                            panel.ShowMessage("�������ܿ�ʼ��Ϸ!");
                        });
                        break;
                    }
                default:
                    {                        
                        break;
                    }
            }
            mainPack = null;
        }
    }

    public void SendRequest()
    {
        MainPack mainPack = new MainPack();
        mainPack.Requestcode = requestCode;
        mainPack.Actioncode = actionCode;
        base.SendRequest(mainPack);
    }


    public override void OnResponse(MainPack pack)
    {
        base.OnResponse(pack);
        this.mainPack = pack;
    }
}
