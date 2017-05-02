using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tnt_deploy;

public class MapLogic:MonoBehaviour
{
    class SprItem
    {
        public string path;
        public Sprite spr;
    }
    List<SprItem> mSprs = new List<SprItem>();
    const int MAP_SPR_COUNT = 3;
    List<SpriteRenderer> mSprRenders = new List<SpriteRenderer>();
    int curSprCount = 0;
//    int curUseSprMaxIdx = 0;
    int curUseSprMinIdx = 0;
//    int curUseMapTrIdx = 0;
    float mSprDistance = 12.8f;
    float mDoubleDistance = 25.6f;
    float mPlayerMoveDistance = 0.0f;
    int mPlayerMoveScreens = 0;
    int mFirstPlayerMoveScreens = 0;
    bool isStart = false;
    int waveIdx = 1;
    float baseMoveRate = 1.0f;
	void Awake()
	{
        for (int i = 1; i <= MAP_SPR_COUNT; i++)
        {
            Transform sprTr = this.transform.FindChild("forest" + i.ToString());
            SpriteRenderer render = sprTr.GetComponent<SpriteRenderer>();
            mSprRenders.Add(render);
        }
        if (mSprRenders.Count >= 2)
        {
            mSprDistance = mSprRenders[1].transform.localPosition.y - mSprRenders[0].transform.localPosition.y;
            BattleInstance.instance.MapLenght = mSprDistance;
            mDoubleDistance = 2 * mSprDistance;
        }
	}
        
	void Start()
	{
//        StartCoroutine(Test());
	}

//    IEnumerator Test()
//    {
////        yield return new WaitForSeconds(1);
//        InitMap(2);
//        isStart = true;
//    }

	void Update()
	{
        if (isStart)
        {
            UpdateMapSprByCameraPos();
        }
	}

	public void InitMap(int mapId)
	{
        Release();
		SCENE_INFO info = DataCfgMgr.instance.GetSceneInfo(mapId);
        curSprCount = info.path.Count;
        for (int i = 0; i < curSprCount; i++) {
            SprItem item = new SprItem();
            item.path = info.path[i];
            item.spr = ResMgr.instance.CreateSprite(item.path);
            mSprs.Add(item);
		}

        int sprIdx = 0;
        for (int i = 0; i < MAP_SPR_COUNT; i++)
        {
            sprIdx = i % curSprCount;
            mSprRenders[i].sprite = mSprs[sprIdx].spr;
        }
        curUseSprMinIdx = 0;
        isStart = true;
//        curUseSprMaxIdx = sprIdx;
//        curUseMapTrIdx = MAP_SPR_COUNT - 1;
	}

    float GetMoveRate()
    {
        float moveMapLength = mPlayerMoveDistance / mSprDistance;
        float moveRate = baseMoveRate + moveMapLength * TestConfig.MapMoveRateAdd;
        if (moveRate > TestConfig.MapMoveMaxRate)
        {
            return TestConfig.MapMoveMaxRate;
        }
        return moveRate;
    }

    void UpdateMapSprByCameraPos()
    {
        // 换地图根据主角，刷兵是根据最前面那个玩家的
        // 摄像机到达第二屏则前面一屏切换到最前面
        mPlayerMoveDistance = Camera.main.transform.localPosition.y - BattleInstance.instance.BaseStartpoint;
        if (mPlayerMoveDistance - mPlayerMoveScreens * mSprDistance >= mSprDistance)
        {
//            curUseSprMaxIdx = (curUseSprMaxIdx + 1) % MAP_SPR_COUNT;
            int preUseMinIdx = curUseSprMinIdx;
            curUseSprMinIdx = (curUseSprMinIdx + 1) % MAP_SPR_COUNT;
//            curUseMapTrIdx = (curUseMapTrIdx + 1) % MAP_SPR_COUNT;
            mPlayerMoveScreens = (int)(mPlayerMoveDistance / mSprDistance);
            float destY = mPlayerMoveScreens * mSprDistance + mDoubleDistance;
            mSprRenders[preUseMinIdx].transform.localPosition = new Vector3(0, destY, 0);

//            waveIdx = waveIdx % DataCfgMgr.instance.GetBrushCarCount() + 1;
//            BattleInstance.instance.SpawnAI(waveIdx);
            BattleInstance.instance.BroadcastToAllUnit((int)Const_Util.BATTLE_EVENT.MOVE_SPEED_RATE, 3 * GetMoveRate());
        }

        if (BattleInstance.instance.GetFirstPlayerDistance() - mFirstPlayerMoveScreens * mSprDistance >= mSprDistance)
        {
            waveIdx = waveIdx % DataCfgMgr.instance.GetBrushCarCount() + 1;
            mFirstPlayerMoveScreens = (int)(BattleInstance.instance.GetFirstPlayerDistance() / mSprDistance);
            BattleInstance.instance.SpawnAI(waveIdx);

            if (waveIdx % 2 == 0)
            {
                SkillBrushMgr.instance.BrushSkillItem();
            }
        }
        else
        {
            if (SkillBrushMgr.instance.HasBrushRandomSkill())
            {
//                float lastDistance = BattleInstance.instance.GetLastPlayerDistance();
                float skillBrushPos = SkillBrushMgr.instance.SkillBrushPosY;
                if (BattleInstance.instance.GetLastPlayerDistance() - SkillBrushMgr.instance.SkillBrushPosY >= mSprDistance)
                {
                    SkillBrushMgr.instance.RecycleBrushItem();
                }
            }
        }
           

//        if (BattleInstance.instance.isBattleStart)
//        {
//            BattleInstance.instance.PlayerMoveDistance = mPlayerMoveDistance;
//        }
    }
        
	public void ChgMap(int sceneID)
	{
        InitMap(sceneID);
	}

    public void Release()
    {
        for (int i = 0; i < mSprs.Count; i++)
        {
            ResMgr.instance.ReleaseSprite(mSprs[i].path);
        }
        mSprs.Clear();
    }
}
