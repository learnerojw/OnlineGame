using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatRequest : BaseRequest
{
    private MainPack mainPack = null;
    private RoomPanel roomPanel;
    public override void Start()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.Chat;
        roomPanel = GetComponent<RoomPanel>();
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
                        roomPanel.UpdateChatText(mainPack);
                        break;
                    }
                case ReturnCode.Fail:
                    {
                        UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                        {
                            panel.ShowMessage("·¢ËÍÏûÏ¢Ê§°Ü");
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

    public override void OnResponse(MainPack pack)
    {
        base.OnResponse(pack);
        this.mainPack = pack;
    }

    public void SendRequest(string chatText)
    {
        MainPack mainPack = new MainPack();
        mainPack.Requestcode = requestCode;
        mainPack.Actioncode = actionCode;
        mainPack.ChatText = chatText;
        base.SendRequest(mainPack);
    }
}
