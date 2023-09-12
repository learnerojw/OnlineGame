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
                this.state.text = "�ȴ�����";
                break;
            case 1:
                this.state.text = "����������";
                break;
            case 2:
                this.state.text = "��Ϸ��";
                break;
        }

    }
}
