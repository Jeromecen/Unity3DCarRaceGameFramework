using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBaseInfoCom : MonoBehaviour {

	// Use this for initialization
    public int mOwnerPlayerIndx = 0;
    public int mOwnerUnitID = 0;
    public int mSkillID = 0;
    public void Init(int playerIdx, int ownerUnitID, int skillID)
    {
        mOwnerPlayerIndx = playerIdx;
        mOwnerUnitID = ownerUnitID;
        mSkillID = skillID;
    }
}
