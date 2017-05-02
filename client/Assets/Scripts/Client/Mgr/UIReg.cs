using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIReg:Singleton<UIReg> {

    public class UIEntity
    {
        public string strName;
        public string resPath;
        public Transform rootTr;
        public CanvasGroup cgCom;
        public int sortingLayer = 0;
        public int orderLayer = 0;

        public System.Type uiType;
        public UIHandler uiHandler = null;
        public int gameObjID = 0;
    }
    public Dictionary<string, UIEntity> mRegUIDict = new Dictionary<string, UIEntity>();

    public const string UIBattle = "ui_battle";
    public void Init()
    {
        int sortingLayer = SortingLayer.NameToID("UI");
        RegisterUI(UIBattle, "Prefabs/UI/UIBattle/ui_battle", typeof(UIBattle), 0, sortingLayer);
    }

    void RegisterUI(string name, string resPath, System.Type handlerType, int orderLayer, int sortingLayer = 3)
    {
        UIEntity entity = new UIEntity();
        entity.strName = name;
        entity.resPath = resPath;
        entity.rootTr = null;
        entity.cgCom = null;
        entity.gameObjID = 0;
        entity.sortingLayer = sortingLayer;
        entity.orderLayer = orderLayer;
        entity.uiType = handlerType;
        entity.uiHandler = null;
        mRegUIDict[name] = entity;
    }

    public UIEntity GetEntity(string name)
    {
        UIEntity entity = null;
        if (mRegUIDict.TryGetValue(name, out entity))
        {
            return entity;
        }
        else
        {
            Debug.LogError("UIReg GetUIEntity entity = null name:" + name);
            return null;
        }
    }
}
