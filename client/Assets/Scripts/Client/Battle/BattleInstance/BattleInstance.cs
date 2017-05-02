using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInstance : Singleton<BattleInstance> {

    const float BattleStartDelayTime = 0.5f;
//    float switchMapTime = BattleStartDelayTime;
    Dictionary<int, Player> mPlayerDict = new Dictionary<int, Player>();
    List<Player> mPlayerList = new List<Player>();

    Transform playerHangNode = null;
    Transform aiHangNode = null;
    Transform otherPlayerHangNode = null;
    public bool isBattleStart = false;
    MapLogic mapLogic = null;

    public class PlayerSortInfo
    {
        public float distance;
        public int playerIdx;
    }
   
    List<PlayerSortInfo> playerSortList = new List<PlayerSortInfo>();
   
    float SortDeltaTime = 0.2f;
    float sortTimer = 0.0f;
    public BattleInstance()
    {
//        BattleStart();
    }
    private float baseStartpoint;
    public float BaseStartpoint
    {
        set;
        get;
    }

    private float mapLenght;
    public float MapLenght
    {
        set;
        get;
    }
//    float playerMoveDistance;
//    public float PlayerMoveDistance
//    {
//        set;
//        get;
//    }
    private int myPlayerIdx;
    public int MyPlayerIdx
    {
        set;
        get;
    }
    public void BattleStart(int mapID)
    {
//        switchMapTime = BattleStartDelayTime;
        if (isBattleStart)
        {
            return;
        }
        playerHangNode = Camera.main.transform;
        Transform mapTr = GameObject.Find("Scene/map").transform;
        aiHangNode = mapTr.FindChild("obstacle").transform;
        otherPlayerHangNode = GameObject.Find("other_player").transform;
        mapLogic = mapTr.GetComponent<MapLogic>();
        if (mapLogic == null)
        {
            mapLogic = mapTr.gameObject.AddComponent<MapLogic>();
        }
        mapLogic.InitMap(mapID);
        InitPlayers();
        isBattleStart = true;
        sortTimer = SortDeltaTime;
    }

    public void BattleEnd()
    {
        // 清理战斗内对象
        Release();
        if (mapLogic != null)
        {
            mapLogic.Release();
        }
        SkillBrushMgr.instance.Release();
        SkillMgr.instance.Release();

        // 隐藏一些UI

    }

    // 从网络传输进来
    void InitPlayers()
    {
        //        PLAYERIDX.PI_AI,  
        //        PI_PLAYER1,
        //        BattleStart();

        int aiIdx = (int)Const_Util.PLAYERIDX.PI_AI;
        Player playerAI = new Player(aiIdx, 0, "AI", aiHangNode);
        playerAI.AddUnitByWave(aiIdx, 1);
        mPlayerDict[aiIdx] = playerAI;
        mPlayerList.Add(playerAI);
        playerAI.SetActive(true);

        Dictionary<int, BattlePlayersInfo.PlayerInfo> playerDict = BattlePlayersInfo.instance.GetPlayerInfoDict();
        int playerIdx = 0;
        int curPlayerIdx = 0;
        foreach (BattlePlayersInfo.PlayerInfo player in playerDict.Values)
        {
            
            if (player.roleID == LoginRoleInfo.instance.RoleID)
            {
                curPlayerIdx = TestConfig.RoleHeroPlayerIdx;
                InitHeroPlayer(curPlayerIdx, player.roleID, player.name, playerHangNode, (int)Const_Util.UNIT_TYPE.ROLE_HERO);
                MyPlayerIdx = curPlayerIdx;
            }
            else
            {
                playerIdx = playerIdx + 1;
                if (playerIdx == TestConfig.RoleHeroPlayerIdx)
                {
                    playerIdx = playerIdx + 1;
                }
                curPlayerIdx = playerIdx;
                InitHeroPlayer(curPlayerIdx, player.roleID, player.name, otherPlayerHangNode, (int)Const_Util.UNIT_TYPE.OTHER_HERO);
            }
            PlayerSortInfo sortInfo = new PlayerSortInfo();
            sortInfo.playerIdx = curPlayerIdx;
            sortInfo.distance = 0.0f;
            playerSortList.Add(sortInfo);
        }
    }

    void InitHeroPlayer(int playerIdx, int roleID, string name, Transform hangNode, int unitType)
    {
        Player player = new Player(playerIdx, roleID, name, hangNode);
        player.InitByHero(playerIdx, unitType);
        mPlayerDict[playerIdx] = player;
        mPlayerList.Add(player);
        player.SetActive(true);
    }

   
    private int PlayerInfoSortFunc(PlayerSortInfo a, PlayerSortInfo b)
    {
        if (a.distance < b.distance)
        {
            return 1;
        }
        else if (a.distance > b.distance)
        {
            return -1;
        }
        return 0;
    }

    void SortPlayerRank(float deltaTime)
    {
        sortTimer = sortTimer - deltaTime;
        if (sortTimer <= 0.0f)
        {
            for(int i = 0; i < playerSortList.Count; i++)
            {
                playerSortList[i].distance = GetHeroCarFloatAttr(playerSortList[i].playerIdx, (int)Const_Util.UNIT_ATTR.MOVE_DISTANCE);
            }
            playerSortList.Sort(PlayerInfoSortFunc);
            sortTimer = SortDeltaTime;
        }
    }

    public float GetFirstPlayerDistance()
    {
        if (playerSortList.Count > 0)
        {
            return playerSortList[0].distance;
        }
        return BattleInstance.instance.BaseStartpoint;
    }

    public float GetLastPlayerDistance()
    {
        if (playerSortList.Count > 0)
        {
            int lastIdx = playerSortList.Count - 1;
            return playerSortList[lastIdx].distance;
        }
        return 0.0f;
    }

    public void Update(float deltaTime)
    {
        if (!isBattleStart)
            return;
        if (mPlayerList.Count > 0)
        {
            for (int i = 0; i < mPlayerList.Count; i++)
            {
                mPlayerList[i].OnUpdate(deltaTime);
            }
        }

        SortPlayerRank(deltaTime);
    }

    public int GetHeroCarID(int playerIdx)
    {
        Player player = GetPlayer(playerIdx);
        if (player == null)
        {
            return 0;
        }
        return player.GetHeroCarID();
    }

    public float GetHeroCarFloatAttr(int playerIdx, int key)
    {
        Player player = GetPlayer(playerIdx);
        if (player == null)
        {
            return 0;
        }
        Vector3 vecValue = Vector3.zero;
        return player.GetHeroCarAttr(key, out vecValue);
    }

    public Vector3 GetHeroCarVecAttr(int playerIdx, int key)
    {
        Player player = GetPlayer(playerIdx);
        if (player == null)
        {
            return Vector3.zero;
        }
        Vector3 vecValue = Vector3.zero;
        player.GetHeroCarAttr(key, out vecValue);
        return vecValue;
    }

    public void BroadcastToAllUnit(int eventID, object args)
    {
        for (int i = 0; i < mPlayerList.Count; i++)
        {
            mPlayerList[i].TriggerEventToAllUnit(eventID, args);
        }
    }

    public Player GetPlayer(int playerIdx )
    {
        Player player = null;
        if(mPlayerDict.TryGetValue(playerIdx, out player))
        {
                return player;
        }
        return null;
    }

    public void SpawnAI(int waveIdx)
    {
        int playerIdx = (int)Const_Util.PLAYERIDX.PI_AI;
        Player player = GetPlayer(playerIdx);
        player.AddUnitByWave(playerIdx, waveIdx);
        player.SetActive(true);
    }

    public void ReleasePlayerUnit(int playerIdx, int objID)
    {
        Player player = null;
        if (mPlayerDict.TryGetValue(playerIdx, out player))
        {
            player.ReleaseUnit(objID);
        }
    }

    void Release()
    {
        isBattleStart = false;
        for (int i = 0; i < mPlayerList.Count; i++)
        {
            mPlayerList[i].Release();
        }
        mPlayerList.Clear();
        mPlayerDict.Clear();
    }
}
