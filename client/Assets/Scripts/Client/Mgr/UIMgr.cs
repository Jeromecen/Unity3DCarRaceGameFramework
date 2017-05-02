using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// UI 管理器，一定要调用统一，自管理，不能导致接口污染
// load是内存加载和卸载
// show是alpha是否显示
public class UIMgr:Singleton<UIMgr>
{
//    public class UIEntity
//    {
//        public string strName;
//        public string resPath;
//        public Transform rootTr;
//        public CanvasGroup cgCom;
//        public int sortingLayer = 0;
//        public int orderLayer = 0;
//        public UIHandler uiHandler = null;
//    }
    List<UIReg.UIEntity> mShowLayerList = new List<UIReg.UIEntity>();
    Dictionary<string, UIReg.UIEntity> mShowLayerDict = new Dictionary<string, UIReg.UIEntity>();
    Transform uiRoot = null;
//    UIReg.UIEntity topEntity = null; // Stack
    public UIMgr()
    {
        GameObject uiRootObj = GameObject.Find("ui_root");
        uiRoot = uiRootObj.transform;
    }

    #region outter interface
    public void ShowUI(string name)
    {
        UIReg.UIEntity entity = null;
        if ( !mShowLayerDict.TryGetValue(name, out entity))
        {
            entity = UIReg.instance.GetEntity(name);
            LoadUI(name);
        }

        if (entity != null)
        {
            ShowHideUIDetail(entity, true);
        }

    }

    public void HideUI(string name, bool forceUnload = false)
    {
        UIReg.UIEntity entity = null;
        if (mShowLayerDict.TryGetValue(name, out entity))
        {
            ShowHideUIDetail(entity, false);
            if (forceUnload)
            {
                UnLoadUI(name);
            }
        }
    }
  
    public bool IsShow(string name)
    {
        UIReg.UIEntity entity = null;
        if (!mShowLayerDict.TryGetValue(name, out entity))
        {
            return entity.cgCom.alpha > Const_Util.FLT_EPSILON;
        }
        return false;
    }

    public void InitMainUI()
    {
        ShowUI(UIReg.UIBattle);
    }

    public void SwithMainToBattle()
    {
    }

    public void SwithBattleToMain()
    {
    }

    public void Update(float deltaTime)
    {
        for (int i = 0; i < mShowLayerList.Count; i++)
        {
            if(mShowLayerList[i].uiHandler != null)
            {
                mShowLayerList[i].uiHandler.OnUpdate(deltaTime);
            }
        }
    }

    #endregion outter interface

    #region innerlogic
    void LoadUI(string name)
    {
        UIReg.UIEntity entity = UIReg.instance.GetEntity(name);
        if (entity.rootTr != null)
        {
            Debug.LogError("UIMgr LoadUI entity.rootTr != null name:" + name);
            return;
        }
        int objID = 0;
        GameObject go = ResMgr.instance.CreateObject(entity.resPath, out objID);
        if (go == null)
        {
            Debug.LogError("UIMgr LoadUI go == nullname:" + name);
            return;
        }
        CanvasGroup cgCom = go.GetComponent<CanvasGroup>();
        if (cgCom == null)
        {
            cgCom = go.AddComponent<CanvasGroup>();
        }
        cgCom.interactable = true;
        cgCom.blocksRaycasts = true;

        go.transform.SetParent(uiRoot);
        UIHandler uiHandler = go.GetComponent(entity.uiType) as UIHandler;
        if (uiHandler == null)
        {
            uiHandler = (go.AddComponent(entity.uiType)) as UIHandler;
        }

        SetEntityValue(entity, objID, go.transform, cgCom, uiHandler);


        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        canvas.sortingLayerID = entity.sortingLayer;
        canvas.sortingOrder = entity.orderLayer;

        uiHandler.OnLoad(go.transform);

        mShowLayerList.Add(entity);
        mShowLayerDict[name] = entity;
    }

    void SetEntityValue(UIReg.UIEntity entity, int objID, Transform rootTr, CanvasGroup cgCom, UIHandler handler)
    {
        entity.gameObjID = objID;
        entity.rootTr = rootTr;
        entity.cgCom = cgCom;
        entity.uiHandler = handler;
    }

    void UnLoadUI(string name)
    {
        UIReg.UIEntity entity = null;
        if (!mShowLayerDict.TryGetValue(name, out entity))
        {
            return;
        }
        entity.uiHandler.OnUnload();
        ResMgr.instance.ReleaseObject(entity.resPath, entity.gameObjID, true);
        mShowLayerList.Remove(entity);
        mShowLayerDict.Remove(name);
        SetEntityValue(entity, 0, null, null, null);
    }

    public void ReleaseAll()
    {
        for (int i = 0; i < mShowLayerList.Count; i++)
        {
            UIReg.UIEntity entity = mShowLayerList[i];
            if (entity.uiHandler != null)
            {
                entity.uiHandler.OnHide();
                entity.uiHandler.OnUnload();
                ResMgr.instance.ReleaseObject(entity.resPath, entity.gameObjID, true);
            }
        }

        mShowLayerList.Clear();
        mShowLayerDict.Clear();
    }

    void ShowHideUIDetail(UIReg.UIEntity entity, bool isShow)
    {
        // todo: can use AysncEvent to scale and fadein or fadeout
        if (isShow)
        {
            if (entity.uiHandler != null)
            {
                entity.uiHandler.OnShow();
            }
            if (entity.cgCom != null)
            {
                entity.cgCom.alpha = 1.0f;
            }
        }
        else
        {
            if (entity.uiHandler != null)
            {
                entity.uiHandler.OnHide();
            }
            if (entity.cgCom != null)
            {
                entity.cgCom.alpha = 0.0f;
            }
        }
       
    }
    #endregion innerlogic
}

