using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UpdatePosRequest : BaseRequest
{
    private MainPack mainPack = null;

    private Transform gunTr;
    public override void Start()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.UpdatePos;

        gunTr = transform.Find("Gun");
        base.Start();
        if(gunTr!=null)
        {
            InvokeRepeating("Send", 1, 1f / 30f);
        }        
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
                        GameFace.Instance.UpdatePos(pack);
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
    
    private void Send()
    {
        SendRequest(transform.position, transform.eulerAngles.z, gunTr.eulerAngles.z);
    }
    public void SendRequest(Vector2 playerPos,float playerRot,float gunRot)
    {
        MainPack mainPack = new MainPack();
        mainPack.Requestcode = requestCode;
        mainPack.Actioncode = actionCode;

        PlayerPack playerPack = new PlayerPack();
        playerPack.PlayerName = GameFace.Instance.GetSelfID();

        PosPack posPack = new PosPack();
        posPack.PosX = playerPos.x;
        posPack.PosY = playerPos.y;
        posPack.RotZ = playerRot;
        posPack.GunRotZ = gunRot;

        playerPack.PosPack = posPack;
        mainPack.PlayerPackList.Add(playerPack);

        //用于UDP传到服务器后，根据用户名查找指定client
        mainPack.UserName = playerPack.PlayerName;
        base.SendToRequest(mainPack);

    }


    public override void OnResponse(MainPack pack)
    {
        if (!isEnabled) return;
        base.OnResponse(pack);
        this.mainPack = pack;
    }
}
