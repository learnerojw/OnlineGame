using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocol;
public class LogonRequest : BaseRequest
{
    private MainPack mainPack = null;
    // Start is called before the first frame update
    public override void Start()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Logon;
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
                        //Debug.Log("登录成功");
                        UIManager.GetInstance().PopPanel("LogonPanel");
                        UIManager.GetInstance().PushPanel<RoomListPanel>("RoomListPanel", (panel) => 
                        {
                            UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                            {
                                panel.ShowMessage("登录成功");
                            });
                        });
                        //登录成功后将用户名记录到客户端的playermanager
                        GameFace.Instance.SetSelfID(mainPack.Loginpack.Username);
                        mainPack = null;
                        break;
                    }
                case ReturnCode.Fail:
                    {
                        //Debug.LogWarning("注册失败");
                        UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                        {
                            panel.ShowMessage("登录失败");
                        });
                        mainPack = null;
                        break;
                    }
                default:
                    {
                        mainPack = null;
                        Debug.Log("既没有成功也没有失败");
                        break;
                    }
            }

            
        }
    }

    public override void OnResponse(MainPack pack)
    {
        base.OnResponse(pack);
        Debug.Log("客户端收到消息");
        //注意啊！！！！！！！！这里超级大坑
        this.mainPack = pack;
    }

    public void SendRequest(string name, string password)
    {
        MainPack pack = new MainPack();
        pack.Requestcode = RequestCode.User;
        pack.Actioncode = ActionCode.Logon;
        LoginPack loginPack = new LoginPack();
        loginPack.Username = name;
        loginPack.Password = password;
        pack.Loginpack = loginPack;
        PlayerPack playerPack = new PlayerPack();
        playerPack.PlayerName = name;
        pack.PlayerPackList.Add(playerPack);
        base.SendRequest(pack);
    }
}
