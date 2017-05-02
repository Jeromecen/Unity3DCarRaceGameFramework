using System;
using System.Collections.Generic;
public class Const_Util:Singleton<Const_Util>
{
    public const float FLT_EPSILON = 0.000001f;
    public enum PLAYERIDX 
    {
        PI_AI = 0,
        PI_PLAYER1 = 1,
        PI_PLAYER2 = 2,
        PI_PLAYER3 = 3,
        PI_PLAYER4 = 4,
        PI_PLAYER5 = 5,
    }

    public enum BATTLE_EVENT
    {
        CHG_DIR = 0,
        MOVE_SPEED_RATE = 1,
        JUMP = 2,
        USE_GAS = 3,
        RANDOM_SKILL = 4,
        USE_SKILL = 5,
    }

    public enum UNIT_MSG
    {
        COLLIDER_OCCUR,
        START_JUMP,
        GAS_ACC,
        GET_SKILL,
        SKILL_DESC,
        SKILL_HIT_EFX,
        SET_STATUS,
    }

    public enum UNIT_TYPE
    {
        AI,
        ROLE_HERO,
        OTHER_HERO,
    }

    public enum UNIT_ATTR
    {
        MOVE_BASE_SPEED = 0,
        MOVE_CUR_SPEED = 1,

        NITROGEN_MAX = 2, // 氮气值
        NITROGEN_CUR = 3,
        UNIT_TYPE = 4,
        MOVE_DISTANCE = 5,
    }
    Dictionary<int, bool> mUnitAttrType = new Dictionary<int, bool>();
    public Const_Util()
    {
        mUnitAttrType[(int)Const_Util.UNIT_ATTR.MOVE_BASE_SPEED] = true;
        mUnitAttrType[(int)Const_Util.UNIT_ATTR.MOVE_CUR_SPEED] = true;
        mUnitAttrType[(int)Const_Util.UNIT_ATTR.NITROGEN_MAX] = false;
        mUnitAttrType[(int)Const_Util.UNIT_ATTR.NITROGEN_CUR] = false;
        mUnitAttrType[(int)Const_Util.UNIT_ATTR.UNIT_TYPE] = false;
        mUnitAttrType[(int)Const_Util.UNIT_ATTR.MOVE_DISTANCE] = false;
    }

    public enum SKILL_TYPE
    {
        SELF_BUFF = 0,
        SPEED_DESC = 1,
        SPEED_STOP = 2,
        SCREEN_PIC = 3,
    }

    public enum SKILL_DIR
    {
        SELF = 0,
        FOWARD = 1,
        BACK = 2,
    }

    public enum UnitStatus
    {
       Normal_Move = 0,
       FLY = 1,
    }


    public enum SKILL_AVOID
    {
        UNKNOWN = 0,
        CHG_ROAD = 1,
        FLY = 2,
    }

    public bool IsAttrVec(int key)
    {
        bool isVec = false;
        mUnitAttrType.TryGetValue(key, out isVec);
        return isVec;
    }
        
}

