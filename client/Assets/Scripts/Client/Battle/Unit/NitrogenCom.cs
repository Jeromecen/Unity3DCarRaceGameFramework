using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitrogenCom:CComponent
{
    AttrCom attrCom = null;
    public NitrogenCom()
    {
    }
        
    public override void  OnMsg(int msgID, object args)
    {
    }

    public override void OnBroadcastEvent(int eventID, int targetObjID, object args = null)
    {
        if ( eventID == (int)Const_Util.BATTLE_EVENT.USE_GAS )
        {
            float curgas = 0.0f;
            Vector3 gasVec = Vector3.zero;
            attrCom.GetAttr((int)Const_Util.UNIT_ATTR.NITROGEN_CUR, out curgas, out gasVec);
            if (curgas > TestConfig.NitragenPerUse)
            {
                curgas -= TestConfig.NitragenPerUse;
                attrCom.SetAttr((int)Const_Util.UNIT_ATTR.NITROGEN_CUR, curgas, gasVec);
                SendMsg((int)Const_Util.UNIT_MSG.GAS_ACC, null);
            }
        }
    }

    public override void OnAttach()
    {
        CComponent acom = cobj.GetCom("AttrCom");
        attrCom = acom as AttrCom;

        RegBroadcastEvent((int)Const_Util.BATTLE_EVENT.USE_GAS);
    }

    public override void Release()
    {
        attrCom = null;
    }

    public override void OnActive(bool active)
    {
    }
        
    public override void OnUpdate(float deltaTime)
    {
        if (attrCom != null)
        {
            float curgas = 0.0f;
            Vector3 gasVec = Vector3.zero;
            attrCom.GetAttr((int)Const_Util.UNIT_ATTR.NITROGEN_CUR, out curgas, out gasVec);

            float maxgas = 0.0f;
            attrCom.GetAttr((int)Const_Util.UNIT_ATTR.NITROGEN_MAX, out maxgas, out gasVec);
            if ( (maxgas - curgas) > Const_Util.FLT_EPSILON )
            {
                curgas += TestConfig.NitragenSpeed * deltaTime;
                attrCom.SetAttr((int)Const_Util.UNIT_ATTR.NITROGEN_CUR, curgas, gasVec);
            }
        }
       
    }

}
