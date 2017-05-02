using System;
using System.Collections;
using System.Collections.Generic;
public class BroadcastEventMgr:Singleton<BroadcastEventMgr>
{
    Dictionary<int, List<IBroadcastEventListener>> mListenerDict = new Dictionary<int, List<IBroadcastEventListener>>();
    // ui targetObjID is zero
    int GetMapIDByMsgIDAndTargetID(int eventID, int targetObjID)
    {
        return eventID * 100000 + targetObjID;
    }

    public void RegBroadcastEvent(int eventID, int targetObjID, IBroadcastEventListener eventObj)
    {
        int mapID = GetMapIDByMsgIDAndTargetID(eventID, targetObjID);
        List<IBroadcastEventListener> msgList = null;
        if (mListenerDict.TryGetValue(mapID, out msgList))
        {
            msgList.Add(eventObj);
        }
        else
        {
            msgList = new List<IBroadcastEventListener>();
            msgList.Add(eventObj);
            mListenerDict[mapID] = msgList;
        }
    }
    public void UnRegBrodcastEvent(int eventID, int targetObjID, IBroadcastEventListener eventObj)
    {
        int mapID = GetMapIDByMsgIDAndTargetID(eventID, targetObjID);
        List<IBroadcastEventListener> msgList = null;
        if (mListenerDict.TryGetValue(mapID, out msgList))
        {
            for (int i = 0; i < msgList.Count; i++)
            {
                if (msgList[i] == eventObj)
                {
                    msgList.Remove(eventObj);
                    if (msgList.Count == 0)
                    {
                        mListenerDict.Remove(mapID);
                    }
                    return;
                }
            }
        }
    }
    // TODO:如果消息转发需要延迟，可以添加delay queue来处理
    public void TriggerEvent(int eventID, int targetObjID, object args)
    {
        int mapID = GetMapIDByMsgIDAndTargetID(eventID, targetObjID);
        List<IBroadcastEventListener> msgList = null;
        if (mListenerDict.TryGetValue(mapID, out msgList))
        {
            for (int i = 0; i < msgList.Count; i++)
            {
                msgList[i].OnBroadcastEvent(eventID, targetObjID, args);
            }
        }
    }
}

