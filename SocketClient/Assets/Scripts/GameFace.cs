using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameFace : MonoBehaviour
{
    private ClientManager clientManager;
    private RequestManager requestManager;
    private PlayerManager playerManager;

    private static GameFace instance;

    public Text text;
    public static GameFace Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance!=this)
        {
            Destroy(gameObject);
        }

        clientManager = new ClientManager(this);
        requestManager = new RequestManager(this);
        playerManager = new PlayerManager(this);
    }

    void Start()
    {
        UIManager.GetInstance().PushPanel<LogonPanel>("LogonPanel");
        clientManager.OnInit();
        requestManager.OnInit();
        playerManager.OnInit();
    }

    private void OnDestroy()
    {
        clientManager.OnDestroy();
        requestManager.OnDestroy();
        playerManager.OnDestroy();
    }
    
    //TCP发送
    public void Send(MainPack pack)
    {
        clientManager.Send(pack);
    }

    public void SendTo(MainPack pack)
    {
        clientManager.SendTo(pack);
    }
    public void HandleResponse(MainPack pack)
    {
        requestManager.HandleResponse(pack);
    }

    public void AddRequest(BaseRequest request)
    {
        requestManager.AddRequest(request);
    }

    public void RemoveRequest(ActionCode action)
    {
        requestManager.RemoveRequest(action);
    }

    public void SetSelfID(string id)
    {
        playerManager.CurPlayerID = id;
    }
    public string GetSelfID()
    {
        return playerManager.CurPlayerID;
    }


    public void AddPlayer(MainPack mainPack)
    {
        playerManager.AddPlayer(mainPack);
    }

    public void RemovePlayer(string id)
    {
        playerManager.removePlayer(id);
    }

    public void GameingExit()
    {
        //移除当前客户端中的所有玩家对象
        playerManager.GameingExit();          
    }

    public void GameingOtherExit(string playerName)
    {
        //移除当前客户端中的指定玩家对象
        playerManager.GameingOtherExit(playerName);
        //移除当前客户端中的指定玩家的UI信息
        if(UIManager.GetInstance().GetPanel<GamePanel>("GamePanel")!=null)
        {
            UIManager.GetInstance().GetPanel<GamePanel>("GamePanel").RemovePlayerUI(playerName);
        }
        
    }

    public void UpdatePos(MainPack mainPack)
    {
        playerManager.UpdatePos(mainPack);
    }

    public void SpawnBullet(MainPack mainPack)
    {
        playerManager.SpawnBullet(mainPack);
    }

    public void Damage(MainPack mainPack)
    {
        PlayerPack playerPack = mainPack.PlayerPackList[0];
        UIManager.GetInstance().GetPanel<GamePanel>("GamePanel").UpdateHP(playerPack.PlayerName, playerPack.Hp);
        playerManager.hitDistance(mainPack);
        
    }
}
