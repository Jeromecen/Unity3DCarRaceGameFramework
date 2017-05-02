using System.Collections;
using System.Collections.Generic;
using System;

public class NetThread{
	// Use this for initialization
    public static void StartListening()
    {
    }
    public static void CloseListening()
    {
    }

    public static void ThreadUpdate(System.Object obj)
    {
        Main mainobj = (Main)obj;
        StartListening();
        BattlePlayersInfo.instance.InitPlayerInfoTest();
        while (mainobj.isRunning)
        {
            // do send queue
            NetSend.SendLogic();
            // do receive msg queue
            NetReceive.ReceiveLogic();

            // sleep
            System.Threading.Thread.Sleep(100);
        }
        CloseListening();
    }
}
