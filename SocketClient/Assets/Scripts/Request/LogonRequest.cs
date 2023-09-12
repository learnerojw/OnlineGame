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
                        //Debug.Log("��¼�ɹ�");
                        UIManager.GetInstance().PopPanel("LogonPanel");
                        UIManager.GetInstance().PushPanel<RoomListPanel>("RoomListPanel", (panel) => 
                        {
                            UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                            {
                                panel.ShowMessage("��¼�ɹ�");
                            });
                        });
                        //��¼�ɹ����û�����¼���ͻ��˵�playermanager
                        GameFace.Instance.SetSelfID(mainPack.Loginpack.Username);
                        mainPack = null;
                        break;
                    }
                case ReturnCode.Fail:
                    {
                        //Debug.LogWarning("ע��ʧ��");
                        UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                        {
                            panel.ShowMessage("��¼ʧ��");
                        });
                        mainPack = null;
                        break;
                    }
                default:
                    {
                        mainPack = null;
                        Debug.Log("��û�гɹ�Ҳû��ʧ��");
                        break;
                    }
            }

            
        }
    }

    public override void OnResponse(MainPack pack)
    {
        base.OnResponse(pack);
        Debug.Log("�ͻ����յ���Ϣ");
        //ע�Ⱑ�������������������ﳬ�����
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
