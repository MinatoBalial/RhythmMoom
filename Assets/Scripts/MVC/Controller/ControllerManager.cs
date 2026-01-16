using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControllerManager
{
    private Dictionary<int, BaseController> _modules; //存控制器的字典

    public ControllerManager()
    {
        _modules = new Dictionary<int, BaseController>();
    }

    //封存枚举里面的东西
    public void Register(ControllerType type,BaseController ctl)
    {
        Register((int)type, ctl);
    }
    //注册控制器
    public void Register(int controllerky,BaseController ctl)
    {
        if (_modules.ContainsKey(controllerky) == false)
        {
            _modules.Add(controllerky, ctl);
        }
    }

    //执行所有控制器Init函数
    public void InitAllModules()
    {
        foreach(var item in _modules)
        {
            item.Value.Init();
        }
    }

    //移除控制器
    public void UnRegister(int controllerkey)
    {
        if (_modules.ContainsKey(controllerkey))
        {
            _modules.Remove(controllerkey);
        }
    }

    //清除
    public void Clear()
    {
        _modules.Clear();
    }

    //清楚所有模块
    public void ClearAllModules()
    {
        List<int> keys = _modules.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            _modules[keys[i]].Destroy();
            _modules.Remove(keys[i]);
        }
    }

    //跨模版触发消息
    public void ApplyFunc(int controllerKey,string eventName, System.Object[] args)
    {
        if (_modules.ContainsKey(controllerKey))
        {
            _modules[controllerKey].ApplyFunc(eventName, args);
        }
    }

    //获取模型的位置
    public BaseModel GetControllerModel(int controllerKey)
    {
        if (_modules.ContainsKey(controllerKey))
        {
            return _modules[controllerKey].GetModel();
        }
        else
        {
            return null;
        }
    }


}
