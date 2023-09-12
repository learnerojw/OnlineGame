using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogonPanel : BasePanel
{
    public InputField username;
    public InputField password;
    public Button logonBtn;
    public Button to_RegisterBtn;
    private LogonRequest logonRequest;
    private void Start()
    {
        to_RegisterBtn.onClick.AddListener(to_Register);
        logonBtn.onClick.AddListener(OnLogonClick);
        logonRequest = GetComponent<LogonRequest>();
    }

    private void to_Register()
    {
        UIManager.GetInstance().PopPanel("LogonPanel");
        UIManager.GetInstance().PushPanel<RegisterPanel>("RegisterPanel");
    }

    private void OnLogonClick()
    {
        if(username.text==""||password.text=="")
        {
            UIManager.GetInstance().PushPanel<MessagePanel>("MessagePanel", (panel) =>
            {
                panel.ShowMessage("用户名或密码不能为空");
            });
            return;
        }
        logonRequest.SendRequest(username.text, password.text);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        username.text = "";
        password.text = "";
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
