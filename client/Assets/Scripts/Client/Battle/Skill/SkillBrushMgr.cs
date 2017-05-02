using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tnt_deploy;

public class SkillBrushMgr : Singleton<SkillBrushMgr> {

	// Use this for initialization
    const string BrushItemResPath = "Prefabs/Skill/skill_random_item";
    const int BrushItemCount = 5;
    class SkillItemObj
    {
        public int objID;
        public GameObject itemObj;
    }
    Dictionary<int, SkillItemObj> brushItemDict = new Dictionary<int, SkillItemObj>();
    private float skillBrushPos;
    public float SkillBrushPosY
    {
        set;
        get;
    }
   
    public bool HasBrushRandomSkill()
    {
        return brushItemDict.Count > 0;
    }

    public SkillBrushMgr()
    {
    }

    public void BrushSkillItem()
    {
        if (brushItemDict.Count > 0)
        {
            return;
        }

        CAR_POS oneCarPos = DataCfgMgr.instance.GetCarPos(1);
        SkillBrushPosY = oneCarPos.car_pos.y + BattleInstance.instance.MapLenght + BattleInstance.instance.GetFirstPlayerDistance();
        for (int i = 1; i <= BrushItemCount; i++)
        {
            int objID = 0;
            CAR_POS carOriginPos = DataCfgMgr.instance.GetCarPos(i);
            GameObject obj = ResMgr.instance.CreateObject(BrushItemResPath, out objID);
            if (obj == null)
            {
                Debug.LogError("ObjMgr SpawnOneUnit obj == null resPath:" + BrushItemResPath);
            }
            Transform objTr = obj.transform;
            Vector3 skillDropPos = new Vector3(carOriginPos.car_pos.x, SkillBrushPosY, 0); 
            objTr.localPosition = skillDropPos;

            SkillItemObj item = new SkillItemObj();
            item.objID = objID;
            item.itemObj = obj;
            brushItemDict[objID] = item;
        }
    }

    public void ReleaseItemByID(int objID)
    {
        SkillItemObj obj = null;
        if (brushItemDict.TryGetValue(objID, out obj))
        {
            ResMgr.instance.ReleaseObject(BrushItemResPath, objID);
            brushItemDict.Remove(objID);
        }
    }

    public void RecycleBrushItem()
    {
        foreach (SkillItemObj obj in brushItemDict.Values)
        {
            ResMgr.instance.ReleaseObject(BrushItemResPath, obj.objID);
        }
        brushItemDict.Clear();
    }

    public void Release()
    {
        RecycleBrushItem();
    }
}
