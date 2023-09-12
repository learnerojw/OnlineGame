using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameExitRequest : BaseRequest
{
    private MainPack mainPack = null;
    public override void Start()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.GameingExit;
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
                        //游戏中途退出
                        
                        GameFace.Instance.GameingExit();
                        UIManager.GetInstance().PopPanel("GamePanel");
                        UIManager.GetInstance().PushPanel<RoomPanel>("RoomPanel");
                        ScenesMgr.GetInstance().LoadScene("OriginalScene", () =>
                        {
                            
                        });
                        break;
                    }
                case ReturnCode.Fail:
                    {
                        
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
        if (!isEnabled) return;
        Debug.Log("执行一次");
        base.OnResponse(pack);
        this.mainPack = pack;
    }

    public void SendRequest()
    {
        MainPack mainPack = new MainPack();
        mainPack.Requestcode = requestCode;
        mainPack.Actioncode = actionCode;
        base.SendRequest(mainPack);
    }
}
