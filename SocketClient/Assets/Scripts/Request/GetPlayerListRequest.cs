using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayerListRequest : BaseRequest
{
    private MainPack mainPack = null;
    private RoomPanel roomPanel;
    public override void Start()
    {
        roomPanel = GetComponent<RoomPanel>();
        requestCode = RequestCode.Room;
        actionCode = ActionCode.GetPlayerList;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (mainPack != null)
        {
            roomPanel.UpdatePlayerList(mainPack);
            mainPack = null;
        }

    }

    public override void OnResponse(MainPack pack)
    {
        mainPack = pack;
        base.OnResponse(pack);
    }

    public void SendRequest()
    {

    }
}
