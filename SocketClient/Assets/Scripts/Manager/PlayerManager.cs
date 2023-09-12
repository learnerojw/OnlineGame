using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : BaseManager
{
    public PlayerManager(GameFace face):base(face)
    {

    }

    private Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();
    private Transform spawnTr;

    public string CurPlayerID
    {
        get;
        set;
    }

    public override void OnInit()
    {
        base.OnInit();

    }

    public void AddPlayer(MainPack mainPack)
    {        
        foreach(PlayerPack playerPack in mainPack.PlayerPackList)
        {
            PlayerPack pack = playerPack;
            ResMgr.GetInstance().LoadAsync<GameObject>("Prefab/Player", (obj) => {
                obj.transform.position = Vector3.zero;
                //如果该用户名等于该客户端用户名，说明这是该客户端可以操控的角色，即添加控制脚本
                if (pack.PlayerName == CurPlayerID)
                {
                    obj.AddComponent<UpdatePosRequest>();
                    //obj.AddComponent<UpdatePos>();
                    obj.AddComponent<PlayerController>();
                    
                }
                else
                {
                    GameObject.Destroy(obj.GetComponent<Rigidbody2D>());
                    PlayerInfo playerInfo= obj.AddComponent<PlayerInfo>();
                    playerInfo.SetInfo(pack.PlayerName);
                }
                Debug.Log(mainPack.PlayerPackList.Count);
                players.Add(pack.PlayerName, obj);
            });            
        }
    }

    public void removePlayer(string id)
    {
        if(players.TryGetValue(id,out GameObject g))
        {
            GameObject.Destroy(g);
            players.Remove(id);
        }
        else
        {
            Debug.Log("移除角色出错");
        }
    }

    public void GameingExit()
    {
        Debug.Log("清除一次玩家物体");
        foreach(GameObject obj in players.Values)
        {
            GameObject.Destroy(obj);
        }
        players.Clear();
    }

    public void GameingOtherExit(string playerName)
    {
        Debug.Log("清除指定玩家物体");
        if (players.ContainsKey(playerName))
        {
            GameObject.Destroy(players[playerName]);
            players.Remove(playerName);
        }
    }

    public void UpdatePos(MainPack mainPack)
    {
        PlayerPack playerPack = mainPack.PlayerPackList[0];
        
        if (players.ContainsKey(playerPack.PlayerName))
        {
            GameObject playerObj = players[playerPack.PlayerName];
            Vector3 pos = new Vector3(playerPack.PosPack.PosX, playerPack.PosPack.PosY, 0);
            //同步位置
            playerObj.transform.position = pos;
            //同步人物旋转
            playerObj.transform.eulerAngles = new Vector3(0,0,playerPack.PosPack.RotZ);
            //同步武器旋转
            playerObj.transform.Find("Gun").eulerAngles = new Vector3(0, 0, playerPack.PosPack.GunRotZ);
        }
    }

    public void SpawnBullet(MainPack mainPack)
    {
        ResMgr.GetInstance().LoadAsync<GameObject>("Prefab/Bullet", (obj) =>
        {
            Vector2 pos = new Vector2(mainPack.BulletPack.PosX, mainPack.BulletPack.PosY);
            obj.transform.position = pos;
            obj.transform.eulerAngles = new Vector3(0, 0, mainPack.BulletPack.RotZ);
        });
    }

    public void hitDistance(MainPack mainPack)
    {
        PlayerPack playerPack = mainPack.PlayerPackList[0];
        //如果受伤的玩家是该客户端玩家，则实现击退
        if(playerPack.PlayerName==CurPlayerID)
        {
            Transform playerTr = players[CurPlayerID].transform;
            //Vector2 bulletPos = new Vector2(mainPack.BulletPack.PosX, mainPack.BulletPack.PosY);
            //Vector2 playerPos = new Vector2(playerTr.position.x, playerTr.position.y);

            Vector2 dir = new Vector2(mainPack.BulletPack.DirX, mainPack.BulletPack.DirY).normalized;
            //playerTr.GetComponent<Rigidbody2D>().velocity+=dir*20;
        }
        //如果血量低于等于0,则移除玩家
        if(playerPack.Hp<=0)
        {
            removePlayer(playerPack.PlayerName);
            if (playerPack.PlayerName == CurPlayerID)
            {
                UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
                {
                    panel.ShowMessage("你已被击败，进入观战状态");
                });
                //为玩家生成观战obj，用于接受其他玩家的同步消息。
                ResMgr.GetInstance().LoadAsync<GameObject>("Prefab/Spectator", (obj) =>
                {

                });
            }
                
        }
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
