using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRequest : BaseRequest
{
    private MainPack mainPack = null;
    public override void Start()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Damage;
        base.Start();
    }

    private void Update()
    {
        if (mainPack != null)
        {
            MainPack pack = mainPack;
            switch (pack.Returncode)
            {
                case ReturnCode.Succeed:
                    {
                        GameFace.Instance.Damage(pack);
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
    public void SendRequest(float posX,float posY,float dirX,float dirY,string hitUserName)
    {
        MainPack mainPack = new MainPack();
        mainPack.Requestcode = requestCode;
        mainPack.Actioncode = actionCode;

        PlayerPack playerPack = new PlayerPack();
        playerPack.PlayerName = hitUserName;
        mainPack.PlayerPackList.Add(playerPack);

        BulletPack bulletPack = new BulletPack();
        bulletPack.PosX = posX;
        bulletPack.PosY = posY;
        bulletPack.DirX = dirX;
        bulletPack.DirY = dirY;
        bulletPack.HitUserName = hitUserName;
        mainPack.BulletPack = bulletPack;

        base.SendRequest(mainPack);
    }
    public override void OnResponse(MainPack pack)
    {
        if (!isEnabled) return;
        base.OnResponse(pack);
        this.mainPack = pack;
    }
}
