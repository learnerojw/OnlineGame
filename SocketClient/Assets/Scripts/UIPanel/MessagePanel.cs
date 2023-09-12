using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : BasePanel
{
    public Text text;
    public float hideTime;
    private float timer;
    //private bool isShow=false;
    public void ShowMessage(string message)
    {
        
        text.text = message;
        text.CrossFadeAlpha(1, 0.1f, false);
        StartCoroutine("HideText");
    }

    IEnumerator HideText()
    {
        timer = hideTime;
        text.CrossFadeAlpha(0, hideTime, false);
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        timer = hideTime;
        UIManager.GetInstance().PopPanel("MessagePanel");
    }

    public override void OnEnter()
    {
        base.OnEnter();
        canvasGroup.interactable = false;
    }

    public override void OnResume()
    {
        base.OnResume();
    }

    public override void OnPause()
    {
        base.OnPause();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
