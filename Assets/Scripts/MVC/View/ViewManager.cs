using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class ViewInfo
{
    public string PrefabName; //视图预制体名称
    public Transform parentTf; //所在父级
    public BaseController controller; //视图所属控制器
    public int Sorting_Order; //显示层级，控制显示优先级
}

public class ViewManager : MonoBehaviour
{
    public Transform canvasTf; //画布组件
    public Transform worldCanvasTf; //世界画布组件
    Dictionary<int, IBaseView> _opens; //已经开启的视图
    Dictionary<int, IBaseView> _viewCache; //视图缓存
    Dictionary<int, ViewInfo> _views; //注册的视图信息

    public ViewManager()
    {
        canvasTf = GameObject.Find("Canvas").transform;
        worldCanvasTf = GameObject.Find("WorldCanvas").transform;
        _opens = new Dictionary<int, IBaseView>();
        _viewCache = new Dictionary<int, IBaseView>();
        _views = new Dictionary<int, ViewInfo>();
    }

    //注册视图信息
    public void Register(int key, ViewInfo viewInfo)
    {
        if (_views.ContainsKey(key) == false)
        {
            _views.Add(key, viewInfo);
        }
    }

    //注册视图信息
    public void Register(ViewType viewType, ViewInfo viewinfo)
    {
        Register((int)viewType, viewinfo);
    }

    //移除视图信息
    public void UnRegister(int key)
    {
        if (_views.ContainsKey(key))
        {
            _views.Remove(key);
        }
    }

    //移除面板
    public void RemoveView(int key)
    {
        _views.Remove(key);
        _viewCache.Remove(key);
        _opens.Remove(key);
    }

    //移除控制器中的面板视图
    public void RemoveViewByController(BaseController ctl)
    {
        foreach (var item in _views)
        {
            if (item.Value.controller == ctl)
            {
                RemoveView(item.Key);
            }
        }
    }

    //是否开启中
    public bool IsOpen(int key)
    {
        return _opens.ContainsKey(key);
    }

    
    public IBaseView GetView(int key) 
    {
        if (_opens.ContainsKey(key))//从已经打开的视图中获取
        {
            return _opens[key];
        }

        if (_viewCache.ContainsKey(key)) //从已经打开的视图中获取
        {
            return _viewCache[key];
        }

        return null;
    }

    public T GetView <T>(int key) where T : class, IBaseView
    {
        IBaseView view = GetView(key);
        if (view != null)
        {
            return view as T;
        }
        return null;
    }

    public void Destroy(int key)
    {
        IBaseView oldView = GetView(key);
        if (oldView != null)
        {
            UnRegister(key);
            oldView.DestroyView();
            _viewCache.Remove(key);
        }
    }

    //关闭面板视图
    public void Close(int key,params object[] args)
    {
        //没有打开
        if(IsOpen(key) == false)
        {
            return;
        }
        IBaseView view = GetView(key);
        if (view != null)
        {
            _opens.Remove(key);
            view.Close(args); //调用视图销毁的时候要做的事情
            _views[key].controller.CloseView(view); //调用控制器，销毁的时候要做的事
        }
    }

    public void CloseAll()
    {
        List<IBaseView> list = _opens.Values.ToList();
        for(int i = list.Count-1; i >= 0; i--)
        {
            Close(list[i].ViewId);
        }
    }

    public void Open(int key, params object[] args)
    {
        IBaseView view = GetView(key);
        ViewInfo viewinfo = _views[key];
        if (view == null)
        {
            //不存在的视图，开始加载资源
            string type = ((ViewType)key).ToString(); //类型的字符串跟脚本的名称对应
            GameObject uiObj = UnityEngine.Object.Instantiate(Resources.Load($"View/{viewinfo.PrefabName}"), viewinfo.parentTf) as GameObject;
            Canvas canvas = uiObj.GetComponent<Canvas>(); //实例化工作
            if(canvas == null )
            {
                canvas = uiObj.AddComponent<Canvas>();
            }

            if (uiObj.GetComponent<GraphicRaycaster>() == null)
            {
                uiObj.AddComponent<GraphicRaycaster>();
            }
            canvas.overrideSorting = true; //可以设置层级
            canvas.sortingOrder = viewinfo.Sorting_Order;//设置层级
            view = uiObj.AddComponent(Type.GetType(type)) as IBaseView;
            view.ViewId = key; //视图id
            view.Controller = viewinfo.controller;
            _viewCache.Add(key, view);
            viewinfo.controller.OnLoadView(view);
        
        }

        //已经打开了
        if(this._opens.ContainsKey(key) == true)
        {
            return;
        }

        this._opens.Add(key, view);

        //已经初始化过的
        if (view.IsInit())
        {
            view.SetVisible(true); //显示
            view.Open(args); //打开
            viewinfo.controller.OpenView(view);
        }
        else
        {
            view.InitUI();
            view.InitData();
            view.Open(args); //调用这个视图的初始化函数
            viewinfo.controller.OpenView(view); //调用控制器开启视图的初始化函数
        }
    }

    public void ShowHitNum(string num,Color color,Vector3 pos)
    {
        GameObject obj = UnityEngine.Object.Instantiate(Resources.Load("View/HitNum"),worldCanvasTf) as GameObject;
        obj.transform.position = pos;
        obj.transform.DOMove(pos + Vector3.up * 1.75f, 0.65f).SetEase(Ease.OutBack);
        UnityEngine.Object.Destroy(obj,0.75f);
        Text hitTxt = obj.GetComponent<Text>();
        hitTxt.text = num;
        hitTxt.color = color;
    }

}
