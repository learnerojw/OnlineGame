using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager1<T> where T:new ()
{
    // Start is called before the first frame update
    private static T instance;

    public static T GetInstance()
    {
        if(instance==null)
        {
            instance = new T();
        }
        return instance;
    }
}
