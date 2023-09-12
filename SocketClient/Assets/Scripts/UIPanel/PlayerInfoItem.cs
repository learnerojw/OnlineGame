using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoItem : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private Slider slider;

    public void Set(string str,int v)
    {
        text.text = str;
        slider.value = (float)v/100;
    }
}
