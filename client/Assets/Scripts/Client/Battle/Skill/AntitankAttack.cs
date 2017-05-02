using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tnt_deploy;
public class AntitankAttack: SkillBase
{
    List<SkillEfxItem> efxObjTrs = new List<SkillEfxItem>();
  
    static float casterDuration = 2.0f;
    static int CastObjCount = 3;
    float lastCasterTimer = 0.0f;
   
    int casterTimeOutNum = 0;
    public AntitankAttack()
    {
    }

    public override bool Begin(int playerIdx, int skillID, int attackID, string skillTmpName)
    {
        base.Begin(playerIdx, skillID, attackID, skillTmpName);
        AddEfxObj();
        lastCasterTimer = 0.0f;
        return true;
    }

    void AddEfxObj()
    {
        if (mIsEnd)
        {
            return;
        }
        SkillEfxItem item = CreateAndInitEfxItem(mEfxResPath, TestConfig.UnitAnchoredYOffset);
        efxObjTrs.Add(item);
    }

    public override void Update(float deltaTime)
    {
        if (mIsEnd)
        {
            return;
        }

        if (efxObjTrs.Count < CastObjCount)
        {
            lastCasterTimer = lastCasterTimer + deltaTime;
            if (lastCasterTimer >= casterDuration)
            {
                AddEfxObj();
                lastCasterTimer = 0.0f;
            }
        }

        casterTimeOutNum = 0;
        for (int i = 0; i < efxObjTrs.Count; i++)
        {
            efxObjTrs[i].existTimer -= deltaTime;
            if (efxObjTrs[i].existTimer <= 0.0f)
            {
                casterTimeOutNum++;
                if( efxObjTrs[i].efxTr.gameObject.activeSelf )
                {
                    efxObjTrs[i].efxTr.gameObject.SetActive(false);
                }
            }
            else
            {
                efxObjTrs[i].efxTr.localPosition += efxObjTrs[i].speed * deltaTime;
                if (efxObjTrs[i].efxTr.localPosition.x <= TestConfig.MapLeftBorder)
                {
                    efxObjTrs[i].speed = new Vector3(Mathf.Abs(efxObjTrs[i].speed.x), efxObjTrs[i].speed.y, 0.0f);
                }
                else if (efxObjTrs[i].efxTr.localPosition.x >= TestConfig.MapRightBorder)
                {
                    efxObjTrs[i].speed = new Vector3(-Mathf.Abs(efxObjTrs[i].speed.x), efxObjTrs[i].speed.y, 0.0f);
                }
            }
        }

        if (casterTimeOutNum == CastObjCount)
        {
            End();
        }
    }

    public override void End()
    {
        for (int i = 0; i < efxObjTrs.Count; i++)
        {
            ReleaseEfxItem(efxObjTrs[i]);
        }
        efxObjTrs.Clear();
        base.End();
    }
}
