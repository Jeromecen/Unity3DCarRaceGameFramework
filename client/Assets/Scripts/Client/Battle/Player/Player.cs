using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tnt_deploy;

// 真正有由玩家产生单位，销毁单位由单位本身的逻辑触发，也要回调到这里。
public class Player{

    List<BattleUnit> mUnitList = new List<BattleUnit>();
    Dictionary<int, BattleUnit> mUnitDict = new Dictionary<int, BattleUnit>();
    public int mPlayerIdx = 0;
    public int mPlayerRoleID = 0;
    public string mPlayerName = "";
    Transform parentTr;
    public Player(int playerIdx, int roleID, string playerName, Transform hangNode)
    {
        mPlayerIdx = playerIdx;
        mPlayerRoleID = roleID;
        mPlayerName = playerName;
        parentTr = hangNode;
    }

    public void TriggerEventToAllUnit(int eventID, object args)
    {
        for (int i = 0; i < mUnitList.Count; i++)
        {
            BroadcastEventMgr.instance.TriggerEvent(eventID, mUnitList[i].GetObjID(), args);
        }
    }
    // 从网络传输进来
    public void InitByHero(int playerIdx, int unitType)
    {
        int heroCarID = 1;
        HERO_CAR heroCar = DataCfgMgr.instance.GetHeroCar(heroCarID);
        int unitResID = (int)heroCar.car_id;
        List<BattleUnit> tempList =  BattleUnitFactory.instance.SpawnUnitFactory(playerIdx, unitResID, 0, parentTr, unitType);
        for (int i = 0; i < tempList.Count; i++)
        {
            mUnitList.Add(tempList[i]);
        }
        CachPlayerUnit();
    }

    void CachPlayerUnit()
    {
        for (int i = 0; i < mUnitList.Count; i++)
        {
            BattleUnit unit = mUnitList[i];
            mUnitDict[unit.GetObjID()] = unit;
        }
    }

    public int GetHeroCarID()
    {
//        mUnitList.
        if (mUnitList.Count > 0)
        {
            return mUnitList[0].GetObjID();
        }
        return 0;
    }

    public BattleUnit GetHeroUnit()
    {
        if (mUnitList.Count > 0)
        {
            return mUnitList[0];
        }
        return null;
    }

    public float GetHeroCarAttr(int key, out Vector3 vecValue)
    {
        if (mUnitList.Count > 0)
        {
            float value = 0.0f;
            BattleUnit unit = mUnitList[0];
            AttrCom attrCom = unit.GetCom("AttrCom") as AttrCom;
            attrCom.GetAttr(key, out value, out vecValue);
            return value;
        }
        vecValue = Vector3.zero;
        return 0.0f;
    }

    public void AddUnitByWave(int playerIdx, int wave)
    {
        List<BattleUnit> tempList = BattleUnitFactory.instance.SpawnUnitFactory (playerIdx, 0, wave, parentTr, (int)Const_Util.UNIT_TYPE.AI);
        if (tempList == null)
        {
            return;
        }
        for (int i = 0; i < tempList.Count; i++)
        {
            mUnitList.Add(tempList[i]);
        }

        CachPlayerUnit();
    }

    // 组件回调过来释放
    public void ReleaseUnit(int objID)
    {
        BattleUnit unit = null;
        if (mUnitDict.TryGetValue(objID, out unit))
        {
            BattleUnitFactory.instance.ReleaseUnitObj(unit);
            mUnitDict.Remove(objID);
            mUnitList.Remove(unit);
        }
    }

    public void Release()
    {
        for (int i = 0; i < mUnitList.Count; i++)
        {
            mUnitList[i].Release();
        }
        mUnitList.Clear();
        mUnitDict.Clear();
    }

    public void SetActive(bool active)
    {
        for (int i = 0; i < mUnitList.Count; i++)
        {
            mUnitList[i].SetActive(active);
        }
    }

    public void OnUpdate(float deltaTime)
    {
        if (mUnitList.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < mUnitList.Count; i++)
        {
            mUnitList[i].Update(deltaTime);
        }
    }
}
