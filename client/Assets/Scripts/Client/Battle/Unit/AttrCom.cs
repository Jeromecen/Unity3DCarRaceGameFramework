using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tnt_deploy;

public class AttrCom:CComponent
{
    Dictionary<int, float> mUnitAttr = new Dictionary<int, float>();
    Dictionary<int, Vector3> mUnitAttrVec = new Dictionary<int, Vector3>();
    public AttrCom()
    {
    }

    public void InitAttr(int carAttrID, int type)
    {
        CAR_ATTR car = DataCfgMgr.instance.GetCar(carAttrID);
        if (car == null)
        {
            Debug.LogError("AttrCom car == null carAttrID:" + carAttrID.ToString());
            return;
        }
        SetAttr((int)Const_Util.UNIT_ATTR.MOVE_BASE_SPEED, 0.0f, Vector3.zero);
        SetAttr((int)Const_Util.UNIT_ATTR.MOVE_CUR_SPEED, 0.0f, Vector3.zero);
        SetAttr((int)Const_Util.UNIT_ATTR.NITROGEN_MAX, car.nitrogen, Vector3.zero);
        SetAttr((int)Const_Util.UNIT_ATTR.NITROGEN_CUR, 0, Vector3.zero);
        SetAttr((int)Const_Util.UNIT_ATTR.UNIT_TYPE, type, Vector3.zero);
    }

    public void SetAttr(int key, float value, Vector3 valueVec)
    {
        if (Const_Util.instance.IsAttrVec(key))
        {
            mUnitAttrVec[key] = valueVec;
        }
        else
        {
            mUnitAttr[key] = value;
        }
    }


    public void GetAttr(int key, out float value, out Vector3 valueVec)
    {
        if ( Const_Util.instance.IsAttrVec(key) )
        {
            mUnitAttrVec.TryGetValue(key, out valueVec);
            value = 0;
        }
        else
        {
            mUnitAttr.TryGetValue(key, out value);
            valueVec = Vector3.zero;
        }
    }

    public override void  OnMsg(int msgID, object args)
    {

    }

    public override void OnBroadcastEvent(int eventID, int targetObjID, object args = null)
//    OnBroadcastEvent(int eventID, int targetObjID, object args = null)
    {
    }

    public override void OnAttach()
    {
    }

    public override void Release()
    {
        mUnitAttr.Clear();
        mUnitAttrVec.Clear();
    }

    public override void OnActive(bool active)
    {
    }

    public override void OnUpdate(float deltaTime)
    {
    }


}

