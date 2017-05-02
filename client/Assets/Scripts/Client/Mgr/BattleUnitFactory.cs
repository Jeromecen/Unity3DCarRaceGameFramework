using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tnt_deploy;
// 只有单位的工厂管理器和UI管理器操作ResMgr， 其它地方避免操作，保持接口的简单性
public class BattleUnitFactory : Singleton<BattleUnitFactory>
{

    public List<BattleUnit> SpawnUnitFactory(int playerIdx, int heroCarID, int waveID, Transform hangNode, int unitType)
    {
        if (heroCarID != 0)
        {

            return SpawnPlayerUnit(playerIdx, heroCarID, hangNode, unitType);
        }
        else if (waveID != 0)
        {
            return SpawnNPCUnit(playerIdx, waveID, hangNode, unitType);
        }
        return null;
    }

    BattleUnit SpawnOneUnit(int playerIdx, int carResID, string resPath, Transform hangNode, Vector2 pos, Vector2 speed, int unitType)
    {
        int objID = 0;
        GameObject obj = ResMgr.instance.CreateObject(resPath, out objID);
        if (obj == null)
        {
            Debug.LogError("ObjMgr SpawnOneUnit obj == null resPath:" + resPath);
        }
        BattleUnit unitObj = new BattleUnit(playerIdx, objID, resPath, obj, hangNode, pos, speed, unitType);
        // attr com
        AttrCom attrCom = unitObj.GetRenderObjTr().GetComponent<AttrCom>();
        if (attrCom == null)
        {
            attrCom = unitObj.GetRenderObjTr().gameObject.AddComponent<AttrCom>();
        }
        unitObj.AddCom(attrCom);
        attrCom.InitAttr(carResID, unitType);

        // move com
        MoveCom move = unitObj.GetRenderObjTr().GetComponent<MoveCom>();
        if (move == null)
        {
            move = unitObj.GetRenderObjTr().gameObject.AddComponent<MoveCom>();
        }
        unitObj.AddCom(move);
        move.Init(speed);

        // collider com
        ColliderCom collider = unitObj.GetRenderObjTr().GetComponent<ColliderCom>();
        if (collider == null)
        {
            collider = unitObj.GetRenderObjTr().gameObject.AddComponent<ColliderCom>();
        }
        unitObj.AddCom(collider);
 
        // Scale com
        ScaleCom scaleCom = unitObj.GetRenderObjTr().GetComponent<ScaleCom>();
        if (scaleCom == null)
        {
            scaleCom = unitObj.GetRenderObjTr().gameObject.AddComponent<ScaleCom>();
        }
        unitObj.AddCom(scaleCom);

        // NitrogenCom
        NitrogenCom nitrogenCom = unitObj.GetRenderObjTr().GetComponent<NitrogenCom>();
        if (nitrogenCom == null)
        {
            nitrogenCom = unitObj.GetRenderObjTr().gameObject.AddComponent<NitrogenCom>();
        }
        unitObj.AddCom(nitrogenCom);


        SkillCom skillCom = unitObj.GetRenderObjTr().GetComponent<SkillCom>();
        if (skillCom == null)
        {
            skillCom = unitObj.GetRenderObjTr().gameObject.AddComponent<SkillCom>();
        }
        unitObj.AddCom(skillCom);


        StatusCom statusCom = unitObj.GetRenderObjTr().GetComponent<StatusCom>();
        if (statusCom == null)
        {
            statusCom = unitObj.GetRenderObjTr().gameObject.AddComponent<StatusCom>();
        }
        unitObj.AddCom(statusCom);

        return unitObj;
    }

    List<BattleUnit> SpawnPlayerUnit(int playerIdx, int heroCarID, Transform hangNode, int unitType)
    {
        HERO_CAR heroCar = DataCfgMgr.instance.GetHeroCar(heroCarID);
        int unitResID = (int)heroCar.car_id;


        List<BattleUnit> objList = new List<BattleUnit>();

        CAR_ATTR car = DataCfgMgr.instance.GetCar(unitResID);
        int carPosID = playerIdx;
        CAR_POS carPos = DataCfgMgr.instance.GetCarPos(carPosID);//
        CAR_VELOCITY carVelocity = DataCfgMgr.instance.GetCarVelocity((int)(heroCar.velocity_id));
        CAR_POS carPosY = DataCfgMgr.instance.GetCarPos((int)(heroCar.pos_id));

        BattleUnit obj = SpawnOneUnit(playerIdx, heroCarID, car.res_path, hangNode, new Vector2(carPos.car_pos.x, carPosY.car_pos.y), 
            new Vector2(carVelocity.car_velocity.x, carVelocity.car_velocity.y), unitType);
        objList.Add(obj);
        return objList;
    }

    List<BattleUnit> SpawnNPCUnit(int playerIdx, int waveID, Transform hangNode, int unitType)
    {
        List<BattleUnit> objList = new List<BattleUnit>();
        BRUSH_CAR brushInfo = DataCfgMgr.instance.GetBrushCar(waveID);
        if (brushInfo == null)
        {
            return null;
        }

        for (int i = 0; i < brushInfo.car_list.Count; i++)
        {
            int carID = (int)(brushInfo.car_list[i].car_id);
            CAR_ATTR car = DataCfgMgr.instance.GetCar(carID);
            CAR_POS carPos = DataCfgMgr.instance.GetCarPos((int)(brushInfo.car_list[i].pos_id));
            CAR_VELOCITY carVelocity = DataCfgMgr.instance.GetCarVelocity((int)(brushInfo.car_list[i].velocity_id));
            BattleUnit obj = SpawnOneUnit(playerIdx, carID, car.res_path, hangNode, new Vector2(carPos.car_pos.x, carPos.car_pos.y), 
                new Vector2(carVelocity.car_velocity.x, carVelocity.car_velocity.y), unitType);
            objList.Add(obj);
        }
        return objList;
    }

    public void ReleaseUnitObj(BattleUnit unit)
    {
        if (unit != null)
        {
//            MoveCom move = unit.mGameObj.GetComponent<MoveCom>();
//            if (move != null)
//            {
//                GameObject.Destroy(move);
//            }
            ResMgr.instance.ReleaseObject(unit.mResPath, unit.mObjID, false);
        }

    }
}
