using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;
using System.IO;
using tnt_deploy;

public class DataCfgMgr : Singleton<DataCfgMgr>
{
    
    SCENE_INFO_ARRAY sceneInfo;
    BRUSH_CAR_ARRAY brushCar;
    CAR_ATTR_ARRAY carAttr;
    CAR_POS_ARRAY carPos;
    CAR_VELOCITY_ARRAY carVelocity;
    HERO_CAR_ARRAY heroCar;
    SKILL_ARRAY skills;

    public void Init()
    {
        sceneInfo = ReadOneDataConfig<SCENE_INFO_ARRAY>("tnt_deploy_scene_info");
        brushCar = ReadOneDataConfig<BRUSH_CAR_ARRAY>("tnt_deploy_brush_car");
        carAttr = ReadOneDataConfig<CAR_ATTR_ARRAY>("tnt_deploy_car_attr");
        carPos = ReadOneDataConfig<CAR_POS_ARRAY>("tnt_deploy_car_pos");
        carVelocity = ReadOneDataConfig<CAR_VELOCITY_ARRAY>("tnt_deploy_car_velocity");
        heroCar = ReadOneDataConfig<HERO_CAR_ARRAY>("tnt_deploy_hero_car");
        skills = ReadOneDataConfig<SKILL_ARRAY>("tnt_deploy_skill");
    }

    public SCENE_INFO GetSceneInfo(int id)
    {
		
        return sceneInfo.items[GetItemIdx(id, sceneInfo.items.Count)];
    }

    public BRUSH_CAR GetBrushCar(int id)
    {
        return brushCar.items[GetItemIdx(id, brushCar.items.Count)];
    }

    public int GetBrushCarCount()
    {
        return brushCar.items.Count;
    }

    public CAR_ATTR GetCar(int id)
    {
        return carAttr.items[GetItemIdx(id, carAttr.items.Count)];
    }

    public CAR_POS GetCarPos(int id)
    {
        return carPos.items[GetItemIdx(id, carPos.items.Count)];
    }

    public CAR_VELOCITY GetCarVelocity(int id)
    {
        return carVelocity.items[GetItemIdx(id, carVelocity.items.Count)];
    }

    public HERO_CAR GetHeroCar(int id)
    {
        return heroCar.items[GetItemIdx(id, heroCar.items.Count)];
    }

    public SKILL GetSkill(int id)
    {
        return skills.items[GetItemIdx(id, skills.items.Count)];
    }

    public int GetSkillCount()
    {
        return skills.items.Count;
    }

    private int GetItemIdx(int id, int count)
    {
        id--;
        if (id < 0 || id >= count)
        {
            id = 0;
        }
        return id;
    }

    private T ReadOneDataConfig<T>(string FileName)
    {
        FileStream fileStream;
        fileStream = GetDataFileStream(FileName);
        if (null != fileStream)
        {
            T t = Serializer.Deserialize<T>(fileStream);
            fileStream.Close();
            return t;
        }

        return default(T);
    }

    private FileStream GetDataFileStream(string fileName)
    {
        string filePath = GetDataConfigPath(fileName);
        if (File.Exists(filePath))
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            return fileStream;
        }

        return null;
    }

    private string GetDataConfigPath(string fileName)
    {
        return Application.dataPath + "/DataConfig/" + fileName + ".data";
        ;
    }
}
