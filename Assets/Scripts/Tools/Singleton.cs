using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T>
{
    private static readonly T instance = Activator.CreateInstance<T>();
    // Start is called before the first frame update

    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    //初始化
    public virtual void Init()
    {

    }

    //销毁
    public virtual void OnDestroy()
    {

    }

    //更新
    public virtual void Update(float dt)
    {

    }


}
