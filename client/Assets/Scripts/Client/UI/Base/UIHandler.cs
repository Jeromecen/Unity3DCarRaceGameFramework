using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIHandler: MonoBehaviour, IBroadcastEventListener
{
    List<int> mRegBroadcastList = new List<int>();
    public abstract void OnLoad(Transform root);

    public abstract void OnShow();
    public abstract void OnHide();
    public abstract void OnUpdate(float deltaTime);
    public abstract void OnBroadcastEvent(int eventID, int targetObjID, object args = null);
    public  void RegBroadcastEvent(int eventID)
    {
        BroadcastEventMgr.instance.RegBroadcastEvent(eventID, 0, this);
        mRegBroadcastList.Add(eventID);
    }
        
    public void UnRegBroadcastEvent(int eventID)
    {
        BroadcastEventMgr.instance.UnRegBrodcastEvent(eventID, 0, this);
    }

    public virtual void OnUnload()
    {
        for (int i = 0; i < mRegBroadcastList.Count; i++)
        {
            UnRegBroadcastEvent(mRegBroadcastList[i]);
        }
        mRegBroadcastList.Clear();
        if (!string.IsNullOrEmpty(name))
        {
            UIMgr.instance.HideUI(name, true);
        }
    }
}

