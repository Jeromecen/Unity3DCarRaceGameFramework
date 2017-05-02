using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCom : CComponent {

    Vector3 speed = Vector3.zero;
    Vector3 targetSpeed = Vector3.zero;
    Vector3 baseMoveSpeed = Vector3.zero;
    float chgSpeedTime = 0.0f;
   
    enum MoveType
    {
        ACC,
        DESC,
        AVG,
    }
    MoveType moveType = MoveType.ACC;
    Vector3 moveUnUseVec = Vector3.zero;
    Transform moveTr = null;
    Transform moveRenderTr = null;
    Vector3 moveAcc = Vector3.zero;

    bool isActive = false;
    SeqEvent moveOprEvent = null;
    SeqEvent gasMoveEvent = null;
    SeqEvent skillMoveEvent = null;
    float colliderCD = 0.0f;
    AttrCom attrCom = null;
    Transform unitHangNode = null;
    Vector3 hangNodeOriginPos = Vector3.zero;
//    MoveToEventY hangNodeResetEvent = null;
    int unitType = 0;
    BattleMsgDef.SkillHitInfo mSkillHitInfo = new BattleMsgDef.SkillHitInfo(-1, 0.0f, 0.0f, 0.0f, 0.0f);

    void TriggerMoveChg(MoveType typeTemp, Vector3 targetSpeedTemp, float timeTemp)
    {
        moveType = typeTemp;
        targetSpeed = targetSpeedTemp;
        chgSpeedTime = timeTemp;
        if (moveType == MoveType.ACC)
        {
            moveAcc = (targetSpeed - speed) / chgSpeedTime;
        }
        else if (moveType == MoveType.DESC)
        {
            moveAcc = (targetSpeed - speed) / chgSpeedTime;
        }
        else if (moveType == MoveType.AVG)
        {
            moveAcc = Vector3.zero;
        }
    }

    void ResetMoveValue()
    {
        moveType = MoveType.ACC;
        targetSpeed = Vector3.zero;
        chgSpeedTime = 0.0f;
        moveAcc = Vector3.zero;
    }

    void SetMoveSpeed(Vector3 speedVec)
    {
        speed = speedVec;
    }

    public void Init(Vector2 speedRef)
    {
        baseMoveSpeed  = new Vector3(speedRef.x, speedRef.y, 0);
 
        CComponent acom = cobj.GetCom("AttrCom");
        attrCom = acom as AttrCom;
        attrCom.SetAttr((int)Const_Util.UNIT_ATTR.MOVE_BASE_SPEED, 0, baseMoveSpeed);
        attrCom.SetAttr((int)Const_Util.UNIT_ATTR.MOVE_CUR_SPEED, 0, speed);

        unitHangNode = cobj.mGameObj.transform;//.parent;
        float unitTypeTemp = 0;
        Vector3 unitTypeUnUse = Vector3.zero;
        attrCom.GetAttr((int)Const_Util.UNIT_ATTR.UNIT_TYPE, out unitTypeTemp, out unitTypeUnUse);
        unitType = Mathf.RoundToInt(unitTypeTemp);

        moveRenderTr = cobj.GetRenderObjTr();
        if (unitType != (int)Const_Util.UNIT_TYPE.AI)
        {
            if (unitType == (int)Const_Util.UNIT_TYPE.ROLE_HERO)
            {
                moveTr = Camera.main.transform;
            }
            else
            {
                moveTr = cobj.GetRenderObjTr();
            }
            SetMoveSpeed(Vector3.zero);

            TriggerMoveChg(MoveType.ACC, baseMoveSpeed, TestConfig.HeroMoveAccTime);

            RegBroadcastEvent((int)Const_Util.BATTLE_EVENT.CHG_DIR);
            RegBroadcastEvent((int)Const_Util.BATTLE_EVENT.JUMP);
            RegMsg((int)Const_Util.UNIT_MSG.GAS_ACC);
            RegMsg((int)Const_Util.UNIT_MSG.SKILL_DESC);
        }
        else
        {
            moveTr = cobj.GetRenderObjTr();
            SetMoveSpeed(baseMoveSpeed);
            moveType = MoveType.AVG;
        }

        hangNodeOriginPos = unitHangNode.localPosition;
        RegMsg((int)Const_Util.UNIT_MSG.COLLIDER_OCCUR);
        RegBroadcastEvent((int)Const_Util.BATTLE_EVENT.MOVE_SPEED_RATE);
    }

    public bool IsHero()
    {
        return unitType != (int)Const_Util.UNIT_TYPE.AI;
    }

    public override void Release()
    {
        unitHangNode.localPosition = hangNodeOriginPos;
        speed = Vector3.zero;
        moveTr = null;
        isActive = false;
        colliderCD = 0.0f;
        ResetMoveValue();
    }

    public override void  OnMsg(int msgID, object args)
    {
        if (!isActive)
        {
            return;
        }
        if (msgID == (int)Const_Util.UNIT_MSG.COLLIDER_OCCUR)
        {
            colliderCD = TestConfig.ColliderHeroHitBaseCD + TestConfig.ColliderHeroHitBaseCD * TestConfig.ColliderHeroHitCDRate * speed.y;
        }
        else if (msgID == (int)Const_Util.UNIT_MSG.GAS_ACC)
        {
            if (gasMoveEvent != null)
            {
                return;
            }
            gasMoveEvent = new SeqEvent();
            OneTimerEvent oneEvent = new OneTimerEvent(GasMoveSpeedChgUp);
            TimerEvent continueTime = new TimerEvent(TestConfig.NitragenStayTime + TestConfig.NitragenMoveTime);
            gasMoveEvent.AddEvent(oneEvent);
            gasMoveEvent.AddEvent(continueTime);

            OneTimerEvent moveDownEvent = new OneTimerEvent(MoveSpeedChgDown);
            TimerEvent stayTime = new TimerEvent(TestConfig.NitragenMoveTime);
            gasMoveEvent.AddEvent(moveDownEvent);
            gasMoveEvent.AddEvent(stayTime);
            gasMoveEvent.Begin();
        }
        else if (msgID == (int)Const_Util.UNIT_MSG.SKILL_DESC)
        {
            if (skillMoveEvent != null)
            {
                return;
            }
            mSkillHitInfo = (BattleMsgDef.SkillHitInfo)args;
            skillMoveEvent = new SeqEvent();
            OneTimerEvent chgEvent = new OneTimerEvent(SkillMoveSpeedChgDown);
            TimerEvent continueTime = new TimerEvent(mSkillHitInfo.chgTime + mSkillHitInfo.continueTime);
            skillMoveEvent.AddEvent(chgEvent);
            skillMoveEvent.AddEvent(continueTime);

            OneTimerEvent resetSpeedEvent = new OneTimerEvent(SkillMoveSpeedReset);
            TimerEvent stayTime = new TimerEvent(mSkillHitInfo.resetTime); 
            skillMoveEvent.AddEvent(resetSpeedEvent);
            skillMoveEvent.AddEvent(stayTime);
            skillMoveEvent.Begin();
        }
    }

    public void GasMoveSpeedChgUp()
    {
        Vector3 targetSpeedVec = targetSpeed * TestConfig.NitragenMoveSpeedAdd;
        TriggerMoveChg(MoveType.ACC, targetSpeedVec, TestConfig.NitragenMoveTime);
    }

    public void MoveSpeedChgDown()
    {
        TriggerMoveChg(MoveType.DESC, baseMoveSpeed, TestConfig.NitragenMoveTime);
    }

    public void SkillMoveSpeedChgDown()
    {
        if (mSkillHitInfo.IsEmpty())
        {
            return;
        }
        Vector3 skillMoveSpeed = mSkillHitInfo.speedScale * baseMoveSpeed;
        TriggerMoveChg(MoveType.DESC, skillMoveSpeed, mSkillHitInfo.chgTime);
    }

    public void SkillMoveSpeedReset()
    {
        if (mSkillHitInfo.IsEmpty())
        {
            return;
        }
        TriggerMoveChg(MoveType.ACC, baseMoveSpeed, mSkillHitInfo.resetTime);
    }

    public override void OnBroadcastEvent(int eventID, int targetObjID, object args = null)
    {
        if (!isActive)
        {
            return;
        }

        if (eventID == (int)Const_Util.BATTLE_EVENT.CHG_DIR)
        {
            unitHangNode.localPosition = hangNodeOriginPos;
            if (colliderCD > Const_Util.FLT_EPSILON)
            {
                return;
            }
            if (moveOprEvent != null && !moveOprEvent.IsFinish())
            {
                moveOprEvent.Destroy();
                moveOprEvent = null;
            }
            moveOprEvent = new SeqEvent();
            MoveToEvent moveTo = new MoveToEvent(cobj.GetRenderObjTr(), new Vector3((float)args, cobj.GetRenderObjTr().localPosition.y, 0), 0.1f);
            moveOprEvent.AddEvent(moveTo);
            moveOprEvent.Begin();
        }
        else if (eventID == (int)Const_Util.BATTLE_EVENT.MOVE_SPEED_RATE)
        {
            if (!IsHero())
            {
                SetMoveSpeed((float)args * baseMoveSpeed);
            }
        }
        else if (eventID == (int)Const_Util.BATTLE_EVENT.JUMP)
        {
            if (colliderCD > Const_Util.FLT_EPSILON)
            {
                return;
            }
            SendMsg((int)Const_Util.UNIT_MSG.START_JUMP, null);
        }
    }
        
    void HeroMoveSpeedAdd(float deltaTime)
    {
        if (IsHero())
        {
            if (moveType == MoveType.ACC && speed.y < targetSpeed.y || moveType == MoveType.DESC && speed.y > targetSpeed.y)
            {
                speed = speed + moveAcc * deltaTime;
                if (moveType == MoveType.ACC && speed.y >= targetSpeed.y)
                {
                    speed = targetSpeed;
                    moveType = MoveType.AVG;
                }
                else if (moveType == MoveType.DESC && speed.y <= targetSpeed.y)
                {
                    speed = targetSpeed;
                    moveType = MoveType.AVG;
                }
                attrCom.SetAttr((int)Const_Util.UNIT_ATTR.MOVE_CUR_SPEED, 0, speed);
            }
        }
    }

    public override void OnAttach()
    {
        
    }

    public override void OnActive(bool active)
    {
        isActive = active;
    }

    public Vector3 GetCurWorldPos()
    {
        return cobj.GetRenderObjTr().position;
    }

    void UpdateMoveEvent(float deltaTime)
    {
        // move event to avoid crash
        if (moveOprEvent != null)
        {
            moveOprEvent.OnUpdate(deltaTime);
            if (moveOprEvent.IsFinish())
            {
                moveOprEvent = null;
            }
        }

        if (gasMoveEvent != null)
        {
            gasMoveEvent.OnUpdate(deltaTime);
            if (gasMoveEvent.IsFinish())
            {
                gasMoveEvent = null;
            }
        }

        if (skillMoveEvent != null)
        {
            skillMoveEvent.OnUpdate(deltaTime);
            if (skillMoveEvent.IsFinish())
            {
                skillMoveEvent = null;
            }
        }

    }

    public override void OnUpdate(float deltaTime)
    {
        if (!isActive)
        {
            return;
        }
        if (colliderCD > Const_Util.FLT_EPSILON)
        {
            colliderCD = colliderCD - deltaTime;
            if (colliderCD <= 0)
            {
                SetMoveSpeed(Vector3.zero);
                TriggerMoveChg(MoveType.ACC, baseMoveSpeed, TestConfig.ColliderHeroMoveResetTime);
            }
            return;
        }
        UpdateMoveEvent(deltaTime);
        HeroMoveSpeedAdd(deltaTime);

        moveTr.localPosition += speed * deltaTime;
//        UpdateCollisionResetEvent(deltaTime);
        float moveDistance = moveRenderTr.position.y - BattleInstance.instance.BaseStartpoint;
        attrCom.SetAttr((int)Const_Util.UNIT_ATTR.MOVE_DISTANCE, moveDistance, moveUnUseVec);

        // check out of screen
        CheckIsOutOffScreen();
    }

    void CheckIsOutOffScreen()
    {
        if (unitType != (int)Const_Util.UNIT_TYPE.AI)
        {
            return;
        }
        float playerPos = Camera.main.transform.localPosition.y;
        if (playerPos - moveTr.localPosition.y >= BattleInstance.instance.MapLenght)
        {
            OnActive(false);
            OnDestroy();
        }
    }
        
//    void SetCollisionResetEvent()
//    {
//        if (colliderCD <= Const_Util.FLT_EPSILON)
//        {
//            if (hangNodeResetEvent == null)
//            {
//                Vector3 targetPos = new Vector3(unitHangNode.localPosition.x, hangNodeOriginPos.y, 0);
//                if (isHero)
//                {
//                   targetPos = hangNodeOriginPos;
//                }
//                hangNodeResetEvent = new MoveToEventY(unitHangNode, targetPos, 2.0f);
//                hangNodeResetEvent.Begin();
//            }
//        }
//    }

//    void UpdateCollisionResetEvent(float deltaTime)
//    {
//        if (hangNodeResetEvent != null)
//        {
//            hangNodeResetEvent.OnUpdate(deltaTime);
//            if (unitHangNode.localPosition.x < TestConfig.MoveCarLeftOfRace)
//            {
//                unitHangNode.localPosition = new Vector3( TestConfig.MoveCarLeftOfRace, unitHangNode.localPosition.y, 0.0f);
//            }
//            else if(unitHangNode.localPosition.x > TestConfig.MoveCarRightOfRace)
//            {
//                unitHangNode.localPosition = new Vector3( TestConfig.MoveCarRightOfRace, unitHangNode.localPosition.y, 0.0f);
//            }
//
//            if (hangNodeResetEvent.IsFinish())
//            {
//                hangNodeResetEvent = null;
//            }
//        }
//    }

    public void OnDestroy()
    {
        moveOprEvent = null;
        base.DoOnDestroy();
    }
}
