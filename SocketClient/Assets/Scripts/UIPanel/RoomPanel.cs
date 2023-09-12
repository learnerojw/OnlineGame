using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : BasePanel
{
    public Button backBtn, sendBtn, startBtn;
    public InputField inputText;
    public Scrollbar scrollbar;
    public Transform userListTr;
    public Text chatText;
    public Text roomName;
    
    private RoomExitRequest roomExitRequest;
    private ChatRequest chatRequest;
    private StartGameRequest startGameRequest;
    void Start()
    {
        roomExitRequest = GetComponent<RoomExitRequest>();
        chatRequest = GetComponent<ChatRequest>();
        startGameRequest = GetComponent<StartGameRequest>();

        backBtn.onClick.AddListener(OnBackClick);
        sendBtn.onClick.AddListener(OnSendClick);
        startBtn.onClick.AddListener(OnStartClick);
    }

    private void OnBackClick()
    {
        roomExitRequest.SendRequest(roomName.text);
    }

    private void OnSendClick()
    {
        if(inputText.text=="")
        {
            UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
            {
                panel.ShowMessage("发送内容不能为空");                
            });
            return;
        }
        chatRequest.SendRequest(inputText.text);
        inputText.text = "";
    }

    private void OnStartClick()
    {
        startGameRequest.SendRequest();
    }
    // Update is called once per frame
    
    public void UpdatePlayerList(MainPack pack)
    {
        //roomName.text = pack.RoompackList[0].Roomname;
        if(pack== null)
        {
            Debug.Log("666");
        }
        
        for(int i=0;i<userListTr.childCount;i++)
        {
            Destroy(userListTr.GetChild(i).gameObject);
        }

        foreach(PlayerPack playerPack in pack.PlayerPackList)
        {
            ResMgr.GetInstance().LoadAsync<GameObject>("UI/UserItem", (obj) =>
            {
                obj.transform.SetParent(userListTr);
                obj.GetComponent<UserItem>().SetPlayerInfo(playerPack);
            });
        }
    }
    
    public void UpdateChatText(MainPack mainPack)
    {
        chatText.text += mainPack.ChatText + "\n";
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
    }
}
