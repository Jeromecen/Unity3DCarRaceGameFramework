using System;

public class BattleMsgDef
{
    public BattleMsgDef()
    {
    }

    public struct RandomSkillAward
    {
        public int skillID;
        public int playerIdx;
        public RandomSkillAward(int id, int idx)
        {
            skillID = id;
            playerIdx = idx;
        }
    }

    public struct SkillHitInfo
    {
        public int skillType;
        public float speedScale;
        public float chgTime;
        public float continueTime;
        public float resetTime;
        public SkillHitInfo(int type, float scale, float chgT, float continueT, float resetT)
        {
            skillType = type;
            speedScale = scale;
            chgTime = chgT;
            continueTime = continueT;
            resetTime = resetT;
        }
//        public SkillHitInfo()
//        {
//            skillType = -1;
//            speedScale = 0.0f;
//            chgTime = 0.0f;
//            continueTime = 0.0f;
//            resetTime = 0.0f;
//        }
        public bool IsEmpty()
        {
            return skillType == -1;
        }
    }
}

