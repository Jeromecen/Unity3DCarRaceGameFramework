using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CComponent:MonoBehaviour,IBroadcastEventListener{
    public CObject cobj = null;

    public abstract void  OnMsg(int msgID, object args);
    public abstract void OnAttach();
    public abstract void OnActive(bool active);
    public abstract void OnUpdate(float deltaTime);
    public abstract void Release();
    public abstract void OnBroadcastEvent(int eventID, int targetObjID, object args = null);

    List<int> mRegBroadcastList = new List<int>();

    public void AddCom(CComponent com)
    {
        if (cobj != null)
        {
           cobj.AddCom(com);
        }
    }

    public CComponent GetCom(string strComName)
    {
        if (cobj != null)
        {
            return cobj.GetCom(strComName);
        }
        return null;
    }

    public void RegMsg(int msgID)
    {
        if (cobj != null)
        {
            cobj.RegMsg(msgID, this);
        }
    }

    public void SendMsg(int msgID, object args)
    {
        if (cobj != null)
        {
            cobj.SendMsg(msgID, args);
        }
    }
   
    public void RegBroadcastEvent(int eventID)
    {
        if (cobj != null)
        {
            int objID = cobj.GetObjID();
            mRegBroadcastList.Add(eventID);
            BroadcastEventMgr.instance.RegBroadcastEvent(eventID, objID, this);
            // 委托给cobj管理
            cobj.RegEventID(eventID, this);
        }
    }
        
    public void UnRegBroadcastEvent(int eventID)
    {
        if (cobj != null)
        {
            int objID = cobj.GetObjID();
            BroadcastEventMgr.instance.UnRegBrodcastEvent(eventID, objID, this);
            cobj.UnRegEventID(eventID, this);
        }
    }

    public void DoOnDestroy()
    {
        OnDestroy();
    }

    void OnDestroy()
    {
        if (cobj != null)
        {
            cobj.Destory();
            mRegBroadcastList.Clear();
            cobj = null;
        }
    }

}
