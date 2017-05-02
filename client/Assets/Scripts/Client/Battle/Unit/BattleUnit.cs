using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : CObject {

    public Player mOwnerPlayer = null;
    public BattleUnit(int playerIdx, int objID, string resPath, GameObject gameObj, Transform hangNode, Vector2 pos, Vector2 speed, int unitType)
        :base(playerIdx, objID, resPath, gameObj, hangNode, pos, speed, unitType)
    {
        
    }

    public void Update(float deltaTime)
    {
        
        base.OnUpdate(deltaTime);
    }
}
