using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tnt_deploy;

public class ColliderCom : CComponent
{

    float collisionCD = 0.0f;
    float getRandomSkillCD = 0.0f;
    float skillEfxHitCD = 0.0f;
    SkillCom skillCom = null;
    public ColliderCom()
    {
        
    }

    public override void  OnMsg(int msgID, object args)
    {
        
    }

    public override void OnBroadcastEvent(int eventID, int targetObjID, object args = null)
    {
    }

    public override void OnAttach()
    {
    }

    public override void OnActive(bool active)
    {
    }

    public override void OnUpdate(float deltaTime)
    {
        if (collisionCD > 0)
        {
            collisionCD = collisionCD - deltaTime;
        }

        if (getRandomSkillCD > 0)
        {
            getRandomSkillCD = getRandomSkillCD - deltaTime;
        }

        if (skillEfxHitCD > 0)
        {
            skillEfxHitCD = skillEfxHitCD - deltaTime;
        }

    }


    //    void OnCollisionEnter2D(Collision2D other)
    //    {
    //        if (other.transform.tag == "Player")
    //        {
    //            DoOnDestroy();
    //        }
    //        else if (other.transform.tag == "Enemy")
    //        {
    //            SendMsg((int)Const_Util.UNIT_MSG.COLLIDER_OCCUR, null);
    //        }
    //    }
    //
    //    void OnCollisionStay2D(Collision2D other)
    //    {
    //    }
    //
    //    void OnCollisionExit2D(Collision2D other)
    //    {
    //    }
//    public enum UnitStatus
//    {
//        Normal_Move = 0,
//        FLY = 1,
//    }
//
//
//    public enum SKILL_AVOID
//    {
//        UNKNOWN = 0,
//        CHG_ROAD = 1,
//        FLY = 2,
//    }

    bool CheckSKillHitValid(Transform hitUnit, SkillBaseInfoCom skillInfoCom)
    {
        if (skillInfoCom.mOwnerPlayerIndx == cobj.mPlayerIdx)
        {
            return false;
        }

        int skillID = skillInfoCom.mSkillID;
        SKILL skill = DataCfgMgr.instance.GetSkill(skillID);

        StatusCom statusCom = hitUnit.GetComponent<StatusCom>();
        Const_Util.UnitStatus status = statusCom.GetStatus();
        if (skill.avoid_method == (uint)Const_Util.SKILL_AVOID.FLY)
        {
            if (status == Const_Util.UnitStatus.FLY)
            {
                return false;
            }
        }
        //SKILL_AVOID.CHG_ROAD; 开始锁定位置，后面不追踪，改变车道不会发生碰撞
        return true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (skillCom == null)
        {
            skillCom = GetCom("SkillCom") as SkillCom;
        }

        if (skillCom == null)
        {
            return;
        }

        if (other.transform.tag == "SkillDrop" && this.transform.tag == "Player")
        {
            if (getRandomSkillCD <= 0.0f && !skillCom.HasSkill())
            {
                SkillBrushMgr.instance.ReleaseItemByID(other.transform.parent.gameObject.GetInstanceID());
                SendMsg((int)Const_Util.UNIT_MSG.GET_SKILL, null);
                getRandomSkillCD = 1.0f;
            }
        }
        else if (other.transform.tag == "Skill" && this.transform.tag == "Player")
        {
            if (skillEfxHitCD > Const_Util.FLT_EPSILON)
            {
                return;
            }
            SkillBaseInfoCom skillInfoCom = other.transform.parent.GetComponent<SkillBaseInfoCom>();
            if( !CheckSKillHitValid(this.transform, skillInfoCom) )
            {
                return;
            }
            // disable skill efx obj
            if (other.transform.parent != null)
            {
                other.transform.parent.gameObject.SetActive(false);

            }
            int skillID = skillInfoCom.mSkillID;
            SKILL skill = DataCfgMgr.instance.GetSkill(skillID);
            // play hit efx
            SendMsg((int)Const_Util.UNIT_MSG.SKILL_HIT_EFX, skillID);

            BattleMsgDef.SkillHitInfo skillHitEfx = new BattleMsgDef.SkillHitInfo((int)skill.type, skill.speed_scale, 
                skill.chg_time, skill.continue_time, skill.reset_time);
            switch (skill.type)
            {
                case (uint)Const_Util.SKILL_TYPE.SPEED_DESC:
                    {
                        SendMsg((int)Const_Util.UNIT_MSG.SKILL_DESC, skillHitEfx);
                        break;
                    }
                case (uint)Const_Util.SKILL_TYPE.SPEED_STOP:
                    {
                        break;
                    }
                case (uint)Const_Util.SKILL_TYPE.SCREEN_PIC:
                    {
                        break;
                    }
                case (uint)Const_Util.SKILL_TYPE.SELF_BUFF:
                    {
                        break;
                    }
                default:
                    {
                        Debug.LogError("ColliderCom OnTriggerEnter2D undefine (int)skill.type:"+ skill.type.ToString());
                        break;
                    }
            }

            skillEfxHitCD = 1.0f;
        }

        if (collisionCD > Const_Util.FLT_EPSILON)
        {
            return;
        }
        if (other.transform.tag == "Player")
        {
            StatusCom statusCom = other.gameObject.GetComponent<StatusCom>();
            Const_Util.UnitStatus status = statusCom.GetStatus();
            if (status != Const_Util.UnitStatus.FLY)
            {
                if (this.transform.tag == "Enemy")
                {
                    DoOnDestroy();
                }
                else
                {
                    SendMsg((int)Const_Util.UNIT_MSG.COLLIDER_OCCUR, null);
                }
            }
            collisionCD = 1.0f;
        }
        else if (other.transform.tag == "Enemy")
        {
            SendMsg((int)Const_Util.UNIT_MSG.COLLIDER_OCCUR, null);
            collisionCD = 1.0f;
        }


    }

    public override void Release()
    {
        skillCom = null;
    }
	
}
