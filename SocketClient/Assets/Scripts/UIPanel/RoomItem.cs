using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Text title;
    public Text num;
    public Text state;
    private Button joinRoomBtn;

    private JoinRoomRequest joinRoomRequest;
    private void Start()
    {
        joinRoomRequest = GetComponent<JoinRoomRequest>();
        joinRoomBtn = GetComponent<Button>();
        joinRoomBtn.onClick.AddListener(OnJoinClick);
    }

    private void OnJoinClick()
    {
        joinRoomRequest.SendRequest(title.text);
    }

    public void SetRoomInfo(RoomPack roomPack)
    {
        title.text = roomPack.Roomname;
        num.text = roomPack.Curnum + "/" + roomPack.Maxnum;
        switch (roomPack.State)
        {
            case 0:
                this.state.text = "等待加入";
                break;
            case 1:
                this.state.text = "房间已满人";
                break;
            case 2:
                this.state.text = "游戏中";
                break;
        }

    }
}
