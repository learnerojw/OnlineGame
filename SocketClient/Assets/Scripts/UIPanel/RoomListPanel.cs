using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListPanel : BasePanel
{
    public Transform roomListTr;
    public Button backBtn, findBtn, createBtn;
    public InputField roomName;
    public Slider slider;
    public Text setNumUI;

    private CreateRoomRequest createRoomRequest;
    private FindRoomRequest findRoomRequest;
    private void Start()
    {
        createRoomRequest = GetComponent<CreateRoomRequest>();
        findRoomRequest = GetComponent<FindRoomRequest>();
        backBtn.onClick.AddListener(OnBackClick);
        findBtn.onClick.AddListener(OnfindClick);
        createBtn.onClick.AddListener(OnCreateClick);
        slider.onValueChanged.AddListener(OnSetNumChange);
    }

    private void OnBackClick()
    {
        UIManager.GetInstance().PopPanel("RoomListPanel");
        UIManager.GetInstance().PushPanel<LogonPanel>("LogonPanel");
    }

    private void OnfindClick()
    {
        findRoomRequest.SendRequest();
    }

    private void OnCreateClick()
    {
        if(roomName.text=="")
        {
            UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
            {
                panel.ShowMessage("创建的房间名不能为空");
            });
            return;
        }
        createRoomRequest.SendRequest(roomName.text,(int)slider.value);
    }

    private void OnSetNumChange(float value)
    {
        setNumUI.text = "人数设置:" + value.ToString();
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
