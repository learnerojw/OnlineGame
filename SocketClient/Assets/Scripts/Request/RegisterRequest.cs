using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterRequest : BaseRequest
{
    private MainPack mainPack = null;
    public override void Start()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Register;
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
                        Debug.Log("注册成功");
                        UIManager.GetInstance().PopPanel("RegisterPanel");
                        UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                        {
                            panel.ShowMessage("注册成功");
                        });
                        UIManager.GetInstance().PushPanel<LogonPanel>("LogonPanel");
                        break;
                    }
                case ReturnCode.Fail:
                    {
                        Debug.LogWarning("注册失败");
                        UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                        {
                            panel.ShowMessage("注册失败,用户名已存在");
                        });
                        break;
                    }
                default:
                    {
                        Debug.Log("既没有成功也没有失败");
                        break;
                    }
            }
            mainPack = null;
        }
    }
    public override void OnResponse(MainPack pack)
    {
        base.OnResponse(pack);
        Debug.Log("客户端收到消息");
        //注意啊！！！！！！！！这里超级大坑
        this.mainPack = pack;
    }

    public void SendRequest(string name,string password)
    {
        MainPack pack = new MainPack();
        pack.Requestcode = RequestCode.User;
        pack.Actioncode = ActionCode.Register;
        LoginPack loginPack = new LoginPack();
        loginPack.Username = name;
        loginPack.Password = password;
        pack.Loginpack = loginPack;
        base.SendRequest(pack);
    }
}
