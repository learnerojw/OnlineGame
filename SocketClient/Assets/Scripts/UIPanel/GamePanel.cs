using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Transform playerList;
    public Text timeText;
    public Button exitBtn;

    private Dictionary<string, PlayerInfoItem> itemList = new Dictionary<string, PlayerInfoItem>();

    private float startTime;
    private GameExitRequest gameExitRequest;
    private void Start()
    {
        startTime = Time.time;
        gameExitRequest = GetComponent<GameExitRequest>();
        exitBtn.onClick.AddListener(OnExitBtn);
        
    }

    private void OnExitBtn()
    {
        gameExitRequest.SendRequest();
    }

    private void FixedUpdate()
    {
        timeText.text = Mathf.Clamp(300 - (int)(Time.time - startTime), 0, 300).ToString();
    }

    public void UpdateList(MainPack mainPack)
    {
        foreach (PlayerInfoItem item in itemList.Values)
        {
            Destroy(item.gameObject);
        }
        itemList.Clear();
        foreach (PlayerPack playerPack in mainPack.PlayerPackList)
        {
            PlayerPack pack = playerPack;
            ResMgr.GetInstance().LoadAsync<GameObject>("UI/PlayerInfoItem", (obj) => {
                obj.transform.SetParent(playerList);
                PlayerInfoItem item = obj.GetComponent<PlayerInfoItem>();
                item.Set(pack.PlayerName, pack.Hp);
                itemList.Add(pack.PlayerName, item);
            });
            
        }
    }

    public void UpdateHP(string id,int hp)
    {
        if(itemList.TryGetValue(id,out PlayerInfoItem itemInfo))
        {
            itemInfo.Set(id, hp);
        }
        else
        {
            Debug.Log("获取不到对应的角色信息");
        }
    }

    public void RemovePlayerUI(string playerName)
    {
        if (itemList.ContainsKey(playerName))
        {
            Destroy(itemList[playerName].gameObject);
            itemList.Remove(playerName);
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnPause()
    {
        base.OnPause();
    }

    public override void OnResume()
    {
        base.OnResume();
    }

    public override void OnExit()
    {
        base.OnExit();
        foreach(PlayerInfoItem item in itemList.Values)
        {
            Destroy(item.gameObject);
        }
        itemList.Clear();
    }
}
