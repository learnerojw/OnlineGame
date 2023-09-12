using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserItem : MonoBehaviour
{
    [SerializeField]
    private Text playerName;
    // Start is called before the first frame update
    public void SetPlayerInfo(PlayerPack playerPack)
    {
        playerName.text = playerPack.PlayerName;
    }
}
