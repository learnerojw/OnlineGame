using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOtherExitRequest : BaseRequest
{
    private MainPack mainPack = null;
    public override void Start()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.GameingOtherExit;
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
                        GameFace.Instance.GameingOtherExit(mainPack.PlayerPackList[0].PlayerName);

                        mainPack = null;
                        break;
                    }
                case ReturnCode.Fail:
                    {
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
        if (!isEnabled) return;
        base.OnResponse(pack);
        this.mainPack = pack;
    }
}
