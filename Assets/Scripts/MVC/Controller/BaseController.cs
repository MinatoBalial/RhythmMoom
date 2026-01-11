using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController
{
    //字典事件
    private Dictionary<string, System.Action<object[]>> message;

    //模版数据
    protected BaseModel model;

    //构造函数
    public BaseController()
    {
        message = new Dictionary<string, System.Action<object[]>>();
    }


    //注册后调用的初始化函数（要所有控制器初始化后执行）

    public virtual void Init() { }

    // 加载视图
    public virtual void OnLoadView(IBaseView view) { }


    //打开视图
    public virtual void OpenView(IBaseView view) { }

    //关闭视图
    public virtual void CloseView(IBaseView view) { }

    //注册模版事件
    public void RegisterFunc(string eventName, System.Action<object[]> callback)
    {
        if (message.ContainsKey(eventName))
        {
            message[eventName] += callback;
        }
        else
        {
            message.Add(eventName, callback);
        }
    }

    //移除模版事件
    public void UnregisterFunc(string eventName)
    {
        if (message.ContainsKey(eventName))
        {
            message.Remove(eventName);
        }
    }

    //唤醒模版事件
    public void ApplyFunc(string eventName,params object[] args)
    {
        if (message.ContainsKey(eventName))
        {
            message[eventName].Invoke(args); //调用事件的地方
        }
        else

        {
            Debug.Log("error:" + eventName + "dose not exist");
        }
    }

    //触发其他模版的事件
    public void ApplyControllerFunc(int controllerKey,string eventName,params object[] args)
    {
        //GameApp.ControllerManager.ApplyFunc(controllerKey, eventName, args);
    }
    public void ApplyControllerFunc(ControllerType type,string eventName,params object[] args)
    {
        ApplyControllerFunc((int)type, eventName, args);
    }

    public void SetModel(BaseModel model)
    {
        this.model = model;
        this.model.controller = this;
    }

    public BaseModel GetModel()
    {
        return model;
    }

    //获取现在这个的model
    public T GetModel<T>() where T : BaseModel
    {
        return model as T;
    }

    //获取这个控制器指定的model
    public BaseModel GetControllerModel(int controllerKey)
    {
        return null;
        //return GameApp.ControllerManager.GetControllerModel(controllerKey);
    }

}
