using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Main : MonoBehaviour {

	// Use this for initialization
    public bool isRunning = false;
    Thread netThread = null;
    void Awake() 
    {
        isRunning = true;
    }
	void Start () {
//		ResMgr.instance.Test ();
        isRunning = true;
		DataCfgMgr.instance.Init();
        UIReg.instance.Init();
        UIMgr.instance.InitMainUI();

        StartNetThead();
	}

    public void StartNetThead()
    {
        netThread = new Thread(NetThread.ThreadUpdate);
        netThread.Start(this);
    }

	// Update is called once per frame
	void Update () {
        float deltaTime = Time.deltaTime;
        BattleInstance.instance.Update(deltaTime);
        UIMgr.instance.Update(deltaTime);
        SkillMgr.instance.Update(deltaTime);
//		ObjMgr.instance.OnUpdate ();
	}

    void OnDestroy() 
    {
        isRunning = false;
        if (netThread != null)
        {
            netThread.Abort();
            netThread = null;
        }
        // 恰当时候清理，而不是OnDestroy时候
//        UIMgr.instance.ReleaseAll();
//        BattleInstance.instance.BattleEnd();
//        ResMgr.instance.ReleaseAll();
    }
}
