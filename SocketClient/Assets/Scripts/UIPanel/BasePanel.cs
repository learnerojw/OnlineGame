using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class BasePanel : MonoBehaviour
{
    private Dictionary<string, List<UIBehaviour>> controllerDic = new Dictionary<string, List<UIBehaviour>>();

    protected CanvasGroup canvasGroup;
    protected virtual void Awake()
    {
        FindControlInChildren<Button>();
        FindControlInChildren<Scrollbar>();
        FindControlInChildren<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public T GetControl<T>(string name) where T:UIBehaviour
    {
        if(controllerDic.ContainsKey(name))
        {
            for (int i = 0; i < controllerDic[name].Count; i++)
            {
                if (controllerDic[name][i] is T)
                {
                    return controllerDic[name][i] as T;
                }
            }
        }
        return null;
    }
    private void FindControlInChildren<T>() where T:UIBehaviour
    {
        T[] controls = GetComponentsInChildren<T>();
        for(int i=0;i<controls.Length;i++)
        {
            if(controllerDic.ContainsKey(controls[i].gameObject.name))
            {
                controllerDic[controls[i].gameObject.name].Add(controls[i]);
            }
            else
            {
                controllerDic.Add(controls[i].gameObject.name, new List<UIBehaviour>() { controls[i]});
            }
        }
    }
    public virtual void OnEnter()
    {
        gameObject.SetActive(true);
        canvasGroup.interactable = true;
    }

    public virtual void OnPause()
    {
        canvasGroup.interactable = false;
    }

    public virtual void OnResume()
    {
        canvasGroup.interactable = true;
        gameObject.SetActive(true);
    }

    public virtual void OnExit()
    {
        canvasGroup.interactable = false;
        gameObject.SetActive(false);
    }
}
