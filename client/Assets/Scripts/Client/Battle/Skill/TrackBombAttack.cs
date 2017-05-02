using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBombAttack : SkillBase {
    string efxResPath;

    public override bool Begin(int playerIdx, int skillID, int attackID, string skillTmpName)
    {
        base.Begin(playerIdx, skillID, attackID, skillTmpName);

        BattleUnit unit = BattleInstance.instance.GetPlayer(mPlayerIdx).GetHeroUnit();
        if (unit == null)
        {
            return false;
        }
        return true;
    }

    public override void Update(float deltaTime)
    {
    }

    public override void End()
    {
    }
}
