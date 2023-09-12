using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinGameRequest : BaseRequest
{
    private MainPack mainPack = null;
    
    public override void Start()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.JoinGame;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(mainPack!=null)
        {
            MainPack pack = mainPack;
            switch (pack.Returncode)
            {
                case ReturnCode.Succeed:
                    {
                        UIManager.GetInstance().PopPanel("RoomPanel");
                        ScenesMgr.GetInstance().LoadScene("GameScene", () =>
                        {
                            UIManager.GetInstance().PushPanel<GamePanel>("GamePanel", (panel) =>
                            {
                                //for (int i = 0; i < 10; i++)
                                //{
                                //    PlayerPack playerPack = new PlayerPack();
                                //    playerPack.PlayerName = i.ToString();
                                //    playerPack.Hp = 100;
                                //    pack.PlayerPackList.Add(playerPack);
                                //}
                                //更新游戏界面的玩家信息
                                panel.UpdateList(pack);

                                UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                                {
                                    panel.ShowMessage("成功进入游戏");
                                });

                                //将玩家游戏对象添加到场景中
                                GameFace.Instance.AddPlayer(pack);

                                
                            });
                        });
                        
                            //Debug.Log(SceneManager.GetActiveScene().name);
                                               
                        break;
                    }
                case ReturnCode.Fail:
                    {
                        UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                        {
                            panel.ShowMessage("进入游戏失败");
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

    //只需要处理不需要发送请求
    public override void OnResponse(MainPack pack)
    {
        if (!isEnabled) return;
        base.OnResponse(pack);
        this.mainPack = pack;
    }
}
