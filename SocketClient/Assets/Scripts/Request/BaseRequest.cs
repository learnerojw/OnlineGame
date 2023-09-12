using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRequest : MonoBehaviour
{
    protected RequestCode requestCode;
    protected ActionCode actionCode;
    protected bool isEnabled = false;
    public ActionCode GetActionCode
    {
        get
        {
            return actionCode;
        }
    }
    public virtual void Start()
    {
        GameFace.Instance.AddRequest(this);
    }

    private void OnEnable()
    {
        isEnabled = true;
    }

    private void OnDisable()
    {
        isEnabled = false;
    }
    public virtual void OnDestroy()
    {
        Debug.Log("ÒÆ³ýÇëÇó");
        GameFace.Instance.RemoveRequest(actionCode);
    }

    public virtual void OnResponse(MainPack pack)
    {

    }

    public virtual void SendRequest(MainPack pack)
    {
        GameFace.Instance.Send(pack);
    }

    public void SendToRequest(MainPack pack)
    {
        GameFace.Instance.SendTo(pack);
    }
}
