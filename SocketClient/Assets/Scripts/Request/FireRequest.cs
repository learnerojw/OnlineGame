using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRequest : BaseRequest
{
    private MainPack mainPack = null;
    public override void Start()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Fire;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(mainPack!=null)
        {
            MainPack pack = mainPack;
            switch(pack.Returncode)
            {
                case ReturnCode.Succeed:
                    {
                        GameFace.Instance.SpawnBullet(pack);
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

    public void SendRequest(Vector2 pos,float rot)
    {
        MainPack mainpack = new MainPack();
        BulletPack bulletPack = new BulletPack();
        bulletPack.PosX = pos.x;
        bulletPack.PosY = pos.y;
        bulletPack.RotZ = rot;
        mainpack.BulletPack = bulletPack;
        mainpack.Requestcode = requestCode;
        mainpack.Actioncode = actionCode;
        base.SendRequest(mainpack);
    }

    public override void OnResponse(MainPack pack)
    {
        if (!isEnabled) return;
        base.OnResponse(pack);
        this.mainPack = pack;
        
    }
}
