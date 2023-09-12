using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using System.Dynamic;

public class UIManager :BaseManager1<UIManager>
{
    private Dictionary<string, BasePanel> dicUI;

    public Stack<BasePanel> stackPanel;

    private RectTransform canvas;
    public UIManager()
    {
        GameObject obj = ResMgr.GetInstance().Load<GameObject>("UI/Canvas");
        canvas = obj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(obj);

        GameObject obj2 = ResMgr.GetInstance().Load<GameObject>("UI/EventSystem");
        GameObject.DontDestroyOnLoad(obj2);

        dicUI = new Dictionary<string, BasePanel>();
        stackPanel = new Stack<BasePanel>();
    }
    public void PushPanel<T>(string name, UnityAction<T> callBack = null) where T : BasePanel
    {
        if(stackPanel.Count>0)
        {
            BasePanel topPanel = stackPanel.Peek();
            topPanel.OnPause();
        }

        if(dicUI.ContainsKey(name))
        {
            //if (dicUI[name].gameObject.activeInHierarchy) return;

            BasePanel targetPanel = dicUI[name];

            int index = canvas.transform.childCount;
            targetPanel.transform.SetSiblingIndex(index-1);
                        
            stackPanel.Push(targetPanel);
            targetPanel.OnEnter();
            
            if (callBack != null)
            {
                callBack(targetPanel as T);
            }
            return;
        }

        ResMgr.GetInstance().LoadAsync<GameObject>("UI/" + name, (obj) =>
          {
              T targetPanel = obj.GetComponent<T>();
              obj.transform.SetParent(canvas);

              obj.transform.localPosition = Vector3.zero;
              obj.transform.localScale = Vector3.one;

              (obj.transform as RectTransform).anchorMin = Vector2.zero;
              (obj.transform as RectTransform).anchorMax = Vector2.one;

              dicUI.Add(name, targetPanel);
              stackPanel.Push(targetPanel);
              targetPanel.OnEnter();


              if (callBack != null)
              {
                  callBack(targetPanel);
              }

              

          });
    }

    public void PopPanel(string name)
    {        
         if (dicUI.ContainsKey(name))
         {
             //Debug.Log("关闭了面板" + dicUI[name].gameObject.name);
             Debug.Log(stackPanel.Peek().name);

             stackPanel.Peek().OnExit();
             stackPanel.Pop();
             
             
             if (stackPanel.Count > 0)
             {
                 //stackPanel.Push(stackPanel.Peek());
                 stackPanel.Peek().OnResume();
             }
             //GameObject.Destroy(dicUI[name].gameObject);
             //dicUI.Remove(name);

         }
            
    }

    public T GetPanel<T>(string name) where T:BasePanel
    {
        if(dicUI.ContainsKey(name))
        {
            return dicUI[name] as T;
        }
        return null;
    }

    public void ClearPanel()
    {
        dicUI.Clear();
        while(stackPanel.Count>0)
        {
            BasePanel panel= stackPanel.Peek();
            panel.OnExit();
            stackPanel.Pop();
            //GameObject.Destroy(panel.gameObject);
        }
    }

    public void Test()
    {
        Debug.Log("真有你的");
    }
}
