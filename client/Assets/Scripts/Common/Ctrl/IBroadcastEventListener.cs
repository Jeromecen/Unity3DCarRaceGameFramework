using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBroadcastEventListener{
    void RegBroadcastEvent(int eventID);
    // 监听实例销毁前，如果有监听事件，一定要去注册,否则广播管理器可能crash.
    void UnRegBroadcastEvent(int eventID);
    // targetObjID和args都是事件触发者提供的，ui的targetObjID为0，对象则为ID
    void OnBroadcastEvent(int eventID, int targetObjID, object args = null);
}
