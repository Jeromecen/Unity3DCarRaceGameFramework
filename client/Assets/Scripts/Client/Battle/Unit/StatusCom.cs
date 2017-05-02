using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StatusCom: CComponent {

    Const_Util.UnitStatus _status = Const_Util.UnitStatus.Normal_Move;
    public StatusCom()
    {
    }
    public override void  OnMsg(int msgID, object args)
    {
        if (msgID == (int)Const_Util.UNIT_MSG.SET_STATUS)
        {
            Const_Util.UnitStatus statusTemp = (Const_Util.UnitStatus)args;
            SetStatus(statusTemp);
        }
    }

    public override void OnAttach()
    {
        RegMsg((int)Const_Util.UNIT_MSG.SET_STATUS);
    }

    public override void OnActive(bool active)
    {
    }

    void SetStatus(Const_Util.UnitStatus status)
    {
        _status = status;
    }

    public Const_Util.UnitStatus GetStatus()
    {
        return _status;
    }

    public override void OnUpdate(float deltaTime)
    {
    }
    public override void Release()
    {
    }

    public override void OnBroadcastEvent(int eventID, int targetObjID, object args = null)
    {
    }
}
