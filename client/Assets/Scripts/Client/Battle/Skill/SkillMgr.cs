using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using tnt_deploy;

public class SkillMgr : Singleton<SkillMgr> {

    public SkillMgr()
    {
        isEnd = false;
        InitSkill();
    }

    List<SkillBase> mSkillInsts = new List<SkillBase>();
    Dictionary<string, List<SkillBase>> mSkillUnUsePool = new Dictionary<string, List<SkillBase>>();

    Dictionary<string, Type> mSkillTemplateMap = new Dictionary<string, Type>();
    float delayRemoeSkillTime = 10.0f;
    float delayRemoveSkillTimer = 10.0f;
    bool isEnd = false;
    void InitSkill()
    {
        mSkillTemplateMap["Antitank"] = typeof(AntitankAttack);
    }

    public void BeginSkill(int playerIdx, int skillID, int attackID)
    {
        if (isEnd)
        {
            return;
        }
        SKILL skill = DataCfgMgr.instance.GetSkill(skillID);
        if (skill == null)
        {
            return;
        }
        Type skillTemplate = null;
        if (!mSkillTemplateMap.TryGetValue(skill.template, out skillTemplate))
        {
            return;
        }
        SkillBase skillInst = GetSkillFromPool(skill.template);
        if (skillInst == null)
        {
            skillInst = Activator.CreateInstance(skillTemplate) as SkillBase;
        }

        if (skillInst.Begin(playerIdx, skillID, attackID, skill.template))
        {
            mSkillInsts.Add(skillInst);
        }
    }

    SkillBase GetSkillFromPool(string templateName)
    {
        List<SkillBase> tempateList = null;
        if (mSkillUnUsePool.TryGetValue(templateName, out tempateList))
        {
            if (tempateList.Count > 0)
            {
                SkillBase skillInst = tempateList[0];
                tempateList.Remove(skillInst);
                return skillInst;
            }
        }

        return null;
    }

    void AddSkillToPool(string templateName, SkillBase skillInst)
    {
        List<SkillBase> tempateList = null;
        if (mSkillUnUsePool.TryGetValue(templateName, out tempateList))
        {
            tempateList.Add(skillInst);
        }
        else
        {
            tempateList = new List<SkillBase>();
            tempateList.Add(skillInst);
            mSkillUnUsePool[templateName] = tempateList;
        }
    }

    public void Update(float deltaTime)
    {
        for (int i = 0; i < mSkillInsts.Count; i++)
        {
            if ( !mSkillInsts[i].IsFinish())
            {
                mSkillInsts[i].Update(deltaTime);
            }
        }
        delayRemoveSkillTimer = delayRemoveSkillTimer - deltaTime;
        if (delayRemoveSkillTimer <= 0.0f)
        {
            CheckRemoveUnUseSkill();
            delayRemoveSkillTimer = delayRemoeSkillTime;
        }
    }

    void CheckRemoveUnUseSkill()
    {
        List<SkillBase> tempSkillToRemove = new List<SkillBase>();
        for (int i = 0; i < mSkillInsts.Count; i++)
        {
            if (mSkillInsts[i].IsFinish())
            {
                tempSkillToRemove.Add(mSkillInsts[i]);
            }
        }

        for (int j = 0; j < tempSkillToRemove.Count; j++)
        {
            string skillName = tempSkillToRemove[j].GetSkillTemplateName();
            AddSkillToPool(skillName, tempSkillToRemove[j]);
            mSkillInsts.Remove(tempSkillToRemove[j]);
        }
    }

    public void Release()
    {
        for (int i = 0; i < mSkillInsts.Count; i++)
        {
            mSkillInsts[i].End();
        }
        mSkillInsts.Clear();
        mSkillUnUsePool.Clear();
        isEnd = true;
    }
}
