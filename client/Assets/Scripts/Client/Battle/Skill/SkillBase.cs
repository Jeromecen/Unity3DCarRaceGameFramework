using UnityEngine;
using tnt_deploy;
public abstract class SkillBase{
    protected int mPlayerIdx;
    protected int mSkillID;
    protected int mAttackID;
    protected Vector3 mAttackWorldPos;
    protected bool mIsEnd = false;
    protected string mSkillTemplateName;
    protected MoveCom mAttackMoveCom = null;
    protected string mEfxResPath;
    protected class SkillEfxItem
    {
        public Transform efxTr;
        public float existTimer;
        public Vector3 speed;
    }

    #region need overrider method
    public abstract void Update(float deltaTime);
    public virtual bool Begin(int playerIdx, int skillID, int attackID, string skillTmpName)
    {
        mPlayerIdx = playerIdx;
        mSkillID = skillID;
        mAttackID = attackID;
        mIsEnd = false;
        mSkillTemplateName = skillTmpName;
        mAttackMoveCom = GetMoveCom();
        if (mAttackMoveCom == null)
        {
            Debug.LogError("SkillBase mAttackMoveCom == null");
        }
        mEfxResPath = GetEfxResPath();
        if (string.IsNullOrEmpty(mEfxResPath))
        {
            Debug.LogError("SkillBase mEfxResPath IsNullOrEmpty");
        }
            
        return true;
    }

    public virtual void End()
    {
        mPlayerIdx = 0;
        mSkillID = 0;
        mAttackID = 0;
        mAttackWorldPos = Vector3.zero;
        mIsEnd = true;
        mAttackMoveCom = null;
        mEfxResPath = "";
    }
    #endregion 
    protected SkillEfxItem CreateAndInitEfxItem(string resPath, float yOffset)
    {
        int objID;
        GameObject efxObj = ResMgr.instance.CreateObject(resPath, out objID);
        if (efxObj == null)
        {
            Debug.LogError("SkillBase Begin() efxObj == null");
            return null;
        }
        // record efx obj
        SkillBaseInfoCom efxBaseInfoCom = efxObj.GetComponent<SkillBaseInfoCom>();
        if (efxBaseInfoCom == null)
        {
            efxBaseInfoCom = efxObj.AddComponent<SkillBaseInfoCom>();
        }
        efxBaseInfoCom.Init(mPlayerIdx, mAttackID, mSkillID);

        // init position
        ObjectUtil.IdentityObj(efxObj.transform);
        Vector3 worldPos = mAttackMoveCom.GetCurWorldPos();
        efxObj.transform.position = new Vector3(worldPos.x, worldPos.y + yOffset, 0.0f);


        SkillEfxItem item = new SkillEfxItem();
        item.efxTr = efxObj.transform;
        SKILL skill = DataCfgMgr.instance.GetSkill(mSkillID);
        item.existTimer = skill.duration;

        // init speed

        Vector3 carSpeed = BattleInstance.instance.GetHeroCarVecAttr(mPlayerIdx, (int)Const_Util.UNIT_ATTR.MOVE_CUR_SPEED);
        float carSpeedY = carSpeed.y;
        if (carSpeedY < TestConfig.EfxSpeedBaseOnCar)
        {
            carSpeedY = TestConfig.EfxSpeedBaseOnCar;
        }

        Vector3 moveSpeed = new Vector3(skill.efx_velocity.x, skill.efx_velocity.y + carSpeedY, 0.0f);
        // check move to left or right
        if (worldPos.x > 0)
        {
            item.speed = new Vector3(-moveSpeed.x, moveSpeed.y, moveSpeed.z);
        }
        else
        {
            item.speed = moveSpeed;
        }

        return item;
    }

    protected void ReleaseEfxItem(SkillEfxItem item)
    {
        Transform tr = item.efxTr;
        if (tr != null)
        {
            ResMgr.instance.ReleaseObject(mEfxResPath, tr.gameObject.GetInstanceID(), false);
        }
    }

    MoveCom GetMoveCom()
    {
        MoveCom moveCom = null;
        BattleUnit unit = BattleInstance.instance.GetPlayer(mPlayerIdx).GetHeroUnit();
        if (unit == null)
        {
            return null;
        }

        moveCom = unit.GetCom("MoveCom") as MoveCom;
        return moveCom;
    }

    string GetEfxResPath()
    {
        SKILL skill = DataCfgMgr.instance.GetSkill(mSkillID);
        if (skill == null)
        {
            return "";
        }
        return skill.respath;
    }

    public bool IsFinish()
    {
        return mIsEnd;
    }

    public string GetSkillTemplateName()
    {
        return mSkillTemplateName;
    }

    public void Break()
    {
        End();
    }
}
