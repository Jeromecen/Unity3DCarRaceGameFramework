using System.Collections;
using System.Collections.Generic;
public class BattlePlayersInfo : Singleton<BattlePlayersInfo>{
    public class PlayerInfo
    {
        public int roleID;
        public string name;
        public int roleLevel;
        public int carID;
        public PlayerInfo(int roleIDPra, string namePra, int roleLevelPra, int carIDPra)
        {
            roleID = roleIDPra;
            name = namePra;
            roleLevel = roleLevelPra;
            carID = carIDPra;
        }
    }
  
    Dictionary<int, PlayerInfo> playerInfoDict = new Dictionary<int, PlayerInfo>();
    public BattlePlayersInfo()
    {
        
    }

    public void InitPlayerInfoTest()
    {
        for (int i = 1; i <= TestConfig.BATTLE_PLAYER_NUM; i++)
        {
            string playerName = "player" + i;
            PlayerInfo p = new PlayerInfo(i, playerName, 1, 1);
            playerInfoDict[i] = p;
        }
    }

    public Dictionary<int, PlayerInfo> GetPlayerInfoDict()
    {
        return playerInfoDict;
    }
}
