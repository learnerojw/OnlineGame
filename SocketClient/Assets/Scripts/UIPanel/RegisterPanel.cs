using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RegisterPanel : BasePanel
{
    public RegisterRequest RegisterRequest;
    public InputField username, password;
    public Button RegisterBtn;
    public Button BackBtn;
    private void Start()
    {
        RegisterBtn.onClick.AddListener(OnRegisterClick);
        BackBtn.onClick.AddListener(OnBackClick);
        RegisterRequest = GetComponent<RegisterRequest>();
    }

    private void OnRegisterClick()
    {
        if(username.text==""|| password.text=="")
        {
            Debug.LogWarning("用户名或密码不能为空!");
            return;
        }
        RegisterRequest.SendRequest(username.text,password.text);
        
    }

    private void OnBackClick()
    {
        UIManager.GetInstance().PopPanel("RegisterPanel");
        UIManager.GetInstance().PushPanel<LogonPanel>("LogonPanel");
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
        //Debug.Log("关闭了RegisterPanel面板");
    }
}
