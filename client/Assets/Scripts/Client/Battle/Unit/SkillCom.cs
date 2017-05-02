using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tnt_deploy;

public class SkillCom : CComponent {
    public SkillCom()
    {
    }
    List<int> mSkillItem = new List<int>();
    MoveCom moveCom = null;
    Transform  renderCarTr = null;
    class SkillHitEfx
    {
        public string _resPath;
        public GameObject _efxObj;
        public Transform _efxTr;
        public float _time;
        public SkillHitEfx(string path, GameObject obj, float time)
        {
            _resPath = path;
            _efxObj = obj;
            _efxTr = obj.transform;
            _time = time;
        }
    }
    // 击中特效自管理
    List<SkillHitEfx> efxParticles = new List<SkillHitEfx>();

//    List<int> mRandomSkill = new List<int>();
    public override void  OnMsg(int msgID, object args)
    {
        if (msgID == (int)Const_Util.UNIT_MSG.GET_SKILL)
        {
            int skillCount = DataCfgMgr.instance.GetSkillCount();
            int randomID = Random.Range(1, skillCount);
            mSkillItem.Add(randomID);
            BattleMsgDef.RandomSkillAward randomAward = new BattleMsgDef.RandomSkillAward(randomID, cobj.mPlayerIdx);
            BroadcastEventMgr.instance.TriggerEvent((int)Const_Util.BATTLE_EVENT.RANDOM_SKILL, 0, randomAward);
        }
        else if (msgID == (int)Const_Util.UNIT_MSG.SKILL_HIT_EFX)
        {
            int skillID = (int)args;
            SKILL skill = DataCfgMgr.instance.GetSkill(skillID);
            int efxParObjID = 0;
            GameObject efxObj = ResMgr.instance.CreateObject(skill.hit_efx, out efxParObjID);
            float time = ParticleUtil.GetParticleLength(efxObj.transform);
            efxObj.transform.position = renderCarTr.position;
            SkillHitEfx efx = new SkillHitEfx(skill.hit_efx, efxObj, time);
            efxParticles.Add(efx);
        }
    }

    public void AddOneRandomSkill()
    {
        
    }

    public bool HasSkill()
    {
        return mSkillItem.Count > 0;
    }

    public override void OnBroadcastEvent(int eventID, int targetObjID, object args = null)
    {
        if (eventID == (int)Const_Util.BATTLE_EVENT.USE_SKILL)
        {
            if (mSkillItem.Count > 0)
            {
                int skillID = 1;//;
//                use skill
                if (moveCom == null)
                {
                    moveCom = GetCom("MoveCom") as MoveCom;
                }
                SkillMgr.instance.BeginSkill(cobj.mPlayerIdx, skillID, cobj.GetObjID());
                // TODO: use skillID
                mSkillItem.Remove(mSkillItem[0]);
            }
        }
    }
    public override void OnAttach()
    {
        renderCarTr = cobj.GetRenderObjTr();
        RegMsg((int)Const_Util.UNIT_MSG.GET_SKILL);
        RegMsg((int)Const_Util.UNIT_MSG.SKILL_HIT_EFX);
        RegBroadcastEvent((int)Const_Util.BATTLE_EVENT.USE_SKILL);
    }

    public override void OnActive(bool active)
    {
    }

    public override void OnUpdate(float deltaTime)
    {
        for (int i = 0; i < efxParticles.Count; i++)
        {
            efxParticles[i]._time -= deltaTime;
            efxParticles[i]._efxTr.position = renderCarTr.position;
            if (efxParticles[i]._time <= 0.0f)
            {
                ResMgr.instance.ReleaseObject(efxParticles[i]._resPath, efxParticles[i]._efxObj.GetInstanceID(), false);
                efxParticles.Remove(efxParticles[i]);
                break;
            }
        }
    }

    public override void Release()
    {
        for (int i = 0; i < efxParticles.Count; i++)
        {
            ResMgr.instance.ReleaseObject(efxParticles[i]._resPath, efxParticles[i]._efxObj.GetInstanceID(), false);
        }
        mSkillItem.Clear();
    }
}

