using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using tnt_deploy;

public class UIBattle : UIHandler{

	// Use this for initialization
//    Button clickBtn = null;
    float []moveXArr = {-3.81f / 2.0f, -1.87f / 2.0f, 0.0f, 1.87f / 2.0f, 3.81f / 2.0f};
    int MOVE_IDX_NUM = 5;
    int curMoveIdx = 2;
    float mLockMoveTimer = 0;
    const float LOCK_OPR_TIME = 0.15f;
   
    Transform touchBtn = null;
    Text nitrogenTxtCom = null;
    float nitrogenValue = 0.0f;
    Image nitrogenImgCom = null;
    Text moveDistanceTextCom = null;
    List<Sprite> randomSkillSprs = new List<Sprite>();
    List<GameObject> randomSkillObjs = new List<GameObject>();
    string RandomSKillObjPath = "Prefabs/UI/UIBattle/skill_item_frush_template";
    SeqEvent randomSkillSprEvent = null;
    Vector2 randomScrollCellSize = Vector2.zero;
    float randomScrollCellSpace = 0.0f;
    Transform skillScrollTr;
    CanvasGroup skillBtnCG = null;
    float originScrollX = -14.0f;
    Text showSkillIDCom = null;
    RectTransform scrollRectTr = null;
    int randomSKillID = 0;
    public override void OnLoad(Transform root)
    {
        Transform bgTr = root.FindChild("bg");
        touchBtn = bgTr.FindChild("touch_btn");
//        Button touchBtnCom = touchBtn.GetComponent<Button>();
        DragEventListener drag = touchBtn.GetComponent<DragEventListener>();
        drag.onDrag = OnTouchDrag;

        Transform btnLeft = root.FindChild("bottom_left");
        Transform nitrogenBtn = btnLeft.FindChild("btn_nitrogen");
        Button nitrogenBtnCom = nitrogenBtn.GetComponent<Button>();
        nitrogenImgCom = nitrogenBtn.GetComponent<Image>();
        nitrogenImgCom.fillAmount = 0.0f;
        nitrogenBtnCom.onClick.AddListener(OnClickNitrogen);
        nitrogenTxtCom = nitrogenBtn.FindChild("value").GetComponent<Text>();
        nitrogenTxtCom.text = "0";

        Transform topRightTr = root.FindChild("top_right");
        Transform distanceTr = topRightTr.FindChild("bg/distance");
        moveDistanceTextCom = distanceTr.GetComponent<Text>();
        moveDistanceTextCom.text = "";
        Button startTestBtn = topRightTr.FindChild("test_start").GetComponent<Button>();
        startTestBtn.onClick.AddListener(OnClickStart);
        Transform randomSkillTr = topRightTr.FindChild("btn_skill");
        skillBtnCG = randomSkillTr.GetComponent<CanvasGroup>();
        skillBtnCG.alpha = 0.0f;
        Button skillBtn = randomSkillTr.GetComponent<Button>();
        skillBtn.onClick.AddListener(OnClickSkill);
        skillScrollTr = randomSkillTr.FindChild("scroll");
        scrollRectTr = skillScrollTr.GetComponent<RectTransform>();
        originScrollX =  scrollRectTr.anchoredPosition3D.x;
        GridLayoutGroup gridGroup = skillScrollTr.GetComponent<GridLayoutGroup>();
        randomScrollCellSize = gridGroup.cellSize;
        randomScrollCellSpace = gridGroup.spacing.y;
        showSkillIDCom = topRightTr.FindChild("skill_id_test").GetComponent<Text>();
        showSkillIDCom.text = "";
//        skillScrollTr.
        RegBroadcastEvent((int)Const_Util.BATTLE_EVENT.RANDOM_SKILL);

//        Transform topLeftTr = root.FindChild("top_left");
        int skillCount = DataCfgMgr.instance.GetSkillCount();
        for (int i = 1; i <= 2 * skillCount; i++)
        {
            int skillIDIdx = i % skillCount;
            if (skillIDIdx == 0)
            {
                skillIDIdx = skillCount;
            }
            SKILL skill = DataCfgMgr.instance.GetSkill(skillIDIdx);
//            System.Text.Encoding.Default.GetString ( byteArray );
            Sprite spr = ResMgr.instance.CreateSprite(skill.ui_skill_icon);
            randomSkillSprs.Add(spr);

            int objID = 0;
            GameObject skillObj = ResMgr.instance.CreateObject(RandomSKillObjPath, out objID);
            Image skillImgCom = skillObj.GetComponent<Image>();
            skillImgCom.sprite = spr;
            skillObj.transform.SetParent(skillScrollTr);
            skillObj.transform.localPosition = new Vector3(skillObj.transform.localPosition.x, skillObj.transform.localPosition.y, 0.0f);
            skillObj.transform.localScale = Vector3.one;
            randomSkillObjs.Add(skillObj);
        }
//        touchBtnCom.OnMouseDrag = 
    }

    public override void OnUnload()
    {
        int skillCount = DataCfgMgr.instance.GetSkillCount();
        for (int i = 0; i < randomSkillSprs.Count; i++)
        {
            int skillIDIdx = (i+1) % skillCount;
            if (skillIDIdx == 0)
            {
                skillIDIdx = skillCount;
            }
            SKILL skill = DataCfgMgr.instance.GetSkill(skillIDIdx);
            ResMgr.instance.ReleaseSprite(skill.ui_skill_icon, true);

            ResMgr.instance.ReleaseObject(RandomSKillObjPath, randomSkillObjs[i].GetInstanceID());
        }
        randomSkillSprs.Clear();
        randomSkillObjs.Clear();
        base.OnUnload();
    }


    public override void OnShow()
    {
    }

    public override void OnHide()
    {
    }

    public override void OnBroadcastEvent(int evnetID, int targetObjID, object args = null)
    {
        if (evnetID == (int)Const_Util.BATTLE_EVENT.RANDOM_SKILL)
        {
            BattleMsgDef.RandomSkillAward randomAward = (BattleMsgDef.RandomSkillAward)args;
            if (randomAward.playerIdx == TestConfig.RoleHeroPlayerIdx && randomSkillSprEvent == null && randomSKillID == 0)
            {
                scrollRectTr.anchoredPosition3D = new Vector3(originScrollX, 0.0f, 0.0f);
                int skillCount = DataCfgMgr.instance.GetSkillCount();
                randomSKillID = randomAward.skillID;
                int randomSkillShowNum = randomSKillID + skillCount;
                float skillDestPosY = (randomSkillShowNum - 1) * (randomScrollCellSize.y + randomScrollCellSpace);
                // show time
                showSkillIDCom.text = "";//randomSKillID.ToString();
                skillBtnCG.interactable = false;

                randomSkillSprEvent = new SeqEvent();
                CanvasGroupFade fadeIn = new CanvasGroupFade(skillBtnCG, 0.0f, 1.0f, TestConfig.RandomSkillFadeInTime);
                randomSkillSprEvent.AddEvent(fadeIn);

                UIAccMoveEvent accMove = new UIAccMoveEvent(scrollRectTr, new Vector3(originScrollX, skillDestPosY, 0.0f), TestConfig.RandomSkillTime, false);
                randomSkillSprEvent.AddEvent(accMove);

                OneTimerEvent showSkillNameFunc = new OneTimerEvent(ShowRandomSkillName);
                randomSkillSprEvent.AddEvent(showSkillNameFunc);
                randomSkillSprEvent.Begin();
            }
        }
    }

    public void ShowRandomSkillName()
    {
        SKILL skillItem = DataCfgMgr.instance.GetSkill(randomSKillID);
        if (skillItem != null && randomSKillID != 0)
        {
            showSkillIDCom.text = skillItem.name;
            skillBtnCG.interactable = true;
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        if (mLockMoveTimer > 0)
        {
            mLockMoveTimer = mLockMoveTimer - Time.deltaTime;
            if (mLockMoveTimer <= 0)
            {
                mLockMoveTimer = 0;
            }
        }

        if (randomSkillSprEvent != null)
        {
            randomSkillSprEvent.OnUpdate(deltaTime);
            if (randomSkillSprEvent.IsFinish())
            {
                randomSkillSprEvent = null;
            }
        }

        if (Input.GetMouseButtonDown(0)||(Input.touchCount >0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            //            #if IPHONE || ANDROID
            //            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            //            #else
            //            if (EventSystem.current.IsPointerOverGameObject())
            //            #endif
            //                Debug.Log("当前触摸在UI上");
            //
            //            else 
            //                Debug.Log("当前没有触摸在UI上");
        }
        float gasValue = BattleInstance.instance.GetHeroCarFloatAttr(BattleInstance.instance.MyPlayerIdx, (int)Const_Util.UNIT_ATTR.NITROGEN_CUR);
        if (Mathf.Abs(gasValue - nitrogenValue) > 1)
        {
            nitrogenValue = gasValue;
            int showGas = Mathf.RoundToInt(nitrogenValue);
            if (showGas < 0)
            {
                showGas = 0;
            }
            nitrogenTxtCom.text = showGas.ToString();
            float gasMaxValue = BattleInstance.instance.GetHeroCarFloatAttr(BattleInstance.instance.MyPlayerIdx, (int)Const_Util.UNIT_ATTR.NITROGEN_MAX);
            nitrogenImgCom.fillAmount = nitrogenValue / gasMaxValue;
        }

//        int showGas = Mathf.RoundToInt(nitrogenValue);
        string strDistance=String.Format("{0:F}",BattleInstance.instance.GetHeroCarFloatAttr(TestConfig.RoleHeroPlayerIdx, (int)Const_Util.UNIT_ATTR.MOVE_DISTANCE));
        moveDistanceTextCom.text = strDistance;

    }

   public void OnClickNitrogen()
   {
        int playerHeroID = BattleInstance.instance.GetHeroCarID(BattleInstance.instance.MyPlayerIdx);
        BroadcastEventMgr.instance.TriggerEvent((int)Const_Util.BATTLE_EVENT.USE_GAS, playerHeroID, null);
//        Debug.Log("OnClickNitrogen");
   }

   public void OnClickStart()
   {
//        Debug.Log("OnClickStart");
        BattleInstance.instance.BattleStart(2);
//        UIMgr.instance.HideUI(UIReg.UIBattle, true);
   }
   
   public void OnClickSkill()
   {
//      Debug.Log("OnClickSkill");  
        int playerHeroID = BattleInstance.instance.GetHeroCarID(BattleInstance.instance.MyPlayerIdx);
        BroadcastEventMgr.instance.TriggerEvent((int)Const_Util.BATTLE_EVENT.USE_SKILL, playerHeroID, null);
        skillBtnCG.alpha = 0.0f;
        randomSKillID = 0;
        showSkillIDCom.text = "";
   }

    public void OnTouchDrag(PointerEventData eventData)
    {
        if (mLockMoveTimer > Const_Util.FLT_EPSILON)
        {
            return;
        }
        bool isLeftRight = false;
        bool isJump = false;
        if (eventData.delta.y > 5.0f)
        {
            isJump = true;
        }
        else if (eventData.delta.x > 1.0f)
        {
//            Debug.Log("> 0 eventData.delta.x:" + eventData.delta.x.ToString());
//            if (eventData.delta.x > 10.0f)
//            {
//                curMoveIdx = curMoveIdx + 2;
//            }
//            else
//            {
                curMoveIdx = curMoveIdx + 1;
//            }

            if (curMoveIdx >= MOVE_IDX_NUM)
            {
                curMoveIdx = MOVE_IDX_NUM - 1;
            }
            isLeftRight = true;
        }
        else if (eventData.delta.x < -1.0f)
        {
//            Debug.Log("< 0 eventData.delta.x:" + eventData.delta.x.ToString());
//            if (eventData.delta.x < -10.0f)
//            {
//                curMoveIdx = curMoveIdx - 2;
//            }
//            else
//            {
                curMoveIdx = curMoveIdx - 1;
//            }

            if (curMoveIdx < 0)
            {
                curMoveIdx = 0;
            }
            isLeftRight = true;
        }

        int playerHeroID = BattleInstance.instance.GetHeroCarID(BattleInstance.instance.MyPlayerIdx);
        if (isLeftRight)
        {
            BroadcastEventMgr.instance.TriggerEvent((int)Const_Util.BATTLE_EVENT.CHG_DIR, playerHeroID, moveXArr[curMoveIdx]);
        }
        else if (isJump)
        {
            BroadcastEventMgr.instance.TriggerEvent((int)Const_Util.BATTLE_EVENT.JUMP, playerHeroID, null);
        }

        mLockMoveTimer = LOCK_OPR_TIME;
    }
}
