using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CObject{

    List<CComponent> mComList = new List<CComponent>();
    Dictionary<string, CComponent> mComDict = new Dictionary<string, CComponent>();
    Dictionary<int, List<CComponent>> mMsgComsDict = new Dictionary<int, List<CComponent>>();
    Dictionary<int, List<CComponent>> mBroadcastEvent = new Dictionary<int, List<CComponent>>();
   
    public int mObjID = 0;
    public GameObject mGameObj = null;
    public Transform mGameRenderObjTr = null;
    public bool mActive = false;
    public string mResPath = "";
    public int unitType = 0;
    public int mPlayerIdx = 0;
    GameObject efxObj = null;
    const string EfxResPath = "Prefabs/Efx/Battle/peiqi";
    public CObject(int playerIdx, int objID, string resPath, GameObject gameObj, Transform hangNode, Vector2 pos, Vector2 speed, int unitTypeTemp)
    {
        if (gameObj == null)
        {
            Debug.LogError("CObject obj == null mResPath:" + mResPath);
            return;
        }
        mPlayerIdx = playerIdx;
        mGameObj = gameObj;
        mObjID = objID;
        mResPath = resPath;
        unitType = unitTypeTemp;
        Transform objTr = mGameObj.GetComponent<Transform>();
        mGameRenderObjTr = objTr.GetChild(0);
        objTr.SetParent(hangNode);
       
        mActive = mGameObj.activeSelf;
        objTr.localPosition = Vector3.zero;
        objTr.localScale = Vector3.one;
        mGameRenderObjTr.localScale = new Vector3(TestConfig.CAR_SACLE, TestConfig.CAR_SACLE, TestConfig.CAR_SACLE);

        efxObj = null;
        switch (unitType)
        {
            
            case (int)Const_Util.UNIT_TYPE.ROLE_HERO:
                {
                    mGameObj.name = "hero_ctrl";
                    mGameRenderObjTr.localPosition = new Vector3(pos.x, pos.y, 0);// + BattleInstance.instance.PlayerMoveDistance
                    AddEfxObj();
                    break;
                }
            case (int)Const_Util.UNIT_TYPE.AI:
                {
                    mGameObj.name = "obstacle_ctrl";
                    float firstPlayerMoveDistance = BattleInstance.instance.GetFirstPlayerDistance();
                    mGameRenderObjTr.localPosition = new Vector3(pos.x, pos.y + BattleInstance.instance.MapLenght+ firstPlayerMoveDistance, 0);// + BattleInstance.instance.PlayerMoveDistance 
                    break;
                }
            case (int)Const_Util.UNIT_TYPE.OTHER_HERO:
                {
                    mGameObj.name = "hero_ctrl";
                    mGameRenderObjTr.localPosition = new Vector3(pos.x, pos.y + BattleInstance.instance.BaseStartpoint, 0);// + BattleInstance.instance.PlayerMoveDistance
                    AddEfxObj();
                    break;
                }
            default:
                {
                    Debug.LogError("CObject unitType error");
                    break;
                }


        }
        SetActive(false);
    }

    public Transform GetRenderObjTr()
    {
        return  mGameRenderObjTr;
    }
   
    public int GetObjID()
    {
        return mObjID;
    }

    public void AddCom(CComponent com)
    {
        if (com == null)
        {
            Debug.LogError("CObject AddComponent error! com == null");
            return;
        }
        if (mGameObj == null)
        {
            mGameObj = com.gameObject.transform.parent.gameObject;
        }
        else
        {
            if (mGameObj != com.gameObject.transform.parent.gameObject)
            {
                Debug.LogError("CObject AddComponent error! com.gameObject name:" + com.gameObject.transform.parent.gameObject.name);
                return;
            }
        }
        com.cobj = this;
        mComList.Add(com);
        // really add to game object
//        mGameObj.AddComponent(com.GetType());
        mComDict[com.GetType().Name] = com;

        com.OnAttach();
    }

    void AddEfxObj()
    {
        int outObjID = 0;
        Transform efxChildTr = mGameRenderObjTr.FindChild("peiqi");
        if (efxChildTr == null)
        {
            efxObj = ResMgr.instance.CreateObject(EfxResPath, out outObjID);
            efxObj.name = "peiqi";
            efxObj.transform.SetParent(mGameRenderObjTr);
            efxObj.transform.localPosition = Vector3.zero;
            efxObj.transform.localScale = Vector3.one;
        }
    }

    public CComponent GetCom(string comName)
    {
        CComponent com;
        if (mComDict.TryGetValue(comName, out com))
        {
            return com;
        }
        return null;
    }

    public void RegEventID(int id, CComponent com)
    {
        List<CComponent> comList = null;
        if (mBroadcastEvent.TryGetValue(id, out comList))
        {
            comList.Add(com);
        }
        else
        {
            comList = new List<CComponent>();
            comList.Add(com);
            mBroadcastEvent[id] = comList;
        }
      
    }
    public void UnRegEventID(int id, CComponent com)
    {
        List<CComponent> comList = null;
        if (mBroadcastEvent.TryGetValue(id, out comList))
        {
            comList.Remove(com);
            if (comList.Count == 0)
            {
                mBroadcastEvent.Remove(id);
            }
        }
    }

    void UnRegAllEvent()
    {
        if (mBroadcastEvent.Count > 0)
        {
            
            foreach (KeyValuePair<int, List<CComponent>> kv in mBroadcastEvent)
            {
                for (int i = 0; i < kv.Value.Count; i++)
                {
                    BroadcastEventMgr.instance.UnRegBrodcastEvent(kv.Key, mObjID, kv.Value[i]);
                }
            }
            mBroadcastEvent.Clear();
        }

    }

    public void SendMsg(int msgID, object args)
    {
        List<CComponent> coms = null;
        if (mMsgComsDict.TryGetValue(msgID, out coms))
        {
            for (int i = 0; i < coms.Count; i++)
            {
                coms[i].OnMsg(msgID, args);
            }
        }
    }
    public void RegMsg(int msgID, CComponent com)
    {
        List<CComponent> coms = null;
        if (mMsgComsDict.TryGetValue(msgID, out coms))
        {
            coms.Add(com);
        }
        else
        {
            coms = new List<CComponent>();
            coms.Add(com);
            mMsgComsDict[msgID] = coms;
        }
    }

    public void SetActive(bool active)
    {
        mActive = active;
        mGameObj.SetActive(active);
        for (int i = 0; i < mComList.Count; i++)
        {
            mComList[i].OnActive(active);
        }
    }

    public void Release()
    {
        if (mGameObj == null)
        {
            return;
        }
        mActive = false;
        mGameObj.SetActive(false);
        for (int i = 0; i < mComList.Count; i++)
        {
            mComList[i].Release();
        }
        if (efxObj != null)
        {
            ResMgr.instance.ReleaseObject(EfxResPath, efxObj.GetInstanceID());
            efxObj = null;
        }
        BattleInstance.instance.ReleasePlayerUnit(mPlayerIdx, mObjID);
        // ObjMgr 持有
        //        ObjMgr.instance.ReleaseUnitObj(mObjID, mResPath);
        mComList.Clear();
        mComDict.Clear();
        mMsgComsDict.Clear();
        mGameObj = null;
    }

    public void OnUpdate(float deltaTime)
    {
        if (mActive)
        {
            for (int i = 0; i < mComList.Count; i++)
            {
                mComList[i].OnUpdate(deltaTime);
            }
        }
    }

    // 统一由组件调用，其它地方不能调用，单一职责要求
    public void Destory()
    {   
        // BattleInstance 持有
        if (mGameObj != null)
        {
            UnRegAllEvent();
            Release();
        }
    }
}
