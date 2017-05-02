using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCom : CComponent {

    SeqEvent jumpOprEvent = null;
    public ScaleCom()
    {
    }
    public override void OnAttach()
    {
        RegMsg((int)Const_Util.UNIT_MSG.START_JUMP);
    }
    public override void OnActive(bool active)
    {
    }

    public override void  OnMsg(int msgID, object args)
    {
        if (msgID == (int)Const_Util.UNIT_MSG.START_JUMP)
        {
            if (jumpOprEvent != null && !jumpOprEvent.IsFinish())
            {
                return;
            }
            jumpOprEvent = new SeqEvent();
            Vector3 originScale = cobj.GetRenderObjTr().localScale;
            Vector3 destScale = originScale * 1.34f;
            ParallelEvent jumpUpPara = new ParallelEvent();
            ScaleEvent scaleTo = new ScaleEvent(cobj.GetRenderObjTr(), originScale, destScale, 0.4f);
            jumpUpPara.AddEvent(scaleTo);
            SeqEvent upEvent = new SeqEvent();
            TimerEvent upTimeDelay = new TimerEvent(0.1f);
            upEvent.AddEvent(upTimeDelay);
            OneTimerEvent scaletoFunc = new OneTimerEvent(JumpEnough);
            upEvent.AddEvent(scaletoFunc);
            jumpUpPara.AddEvent(upEvent);

            TimerEvent timeDelay = new TimerEvent(0.8f);

            SeqEvent downEvent = new SeqEvent();
            TimerEvent downTimeDelay = new TimerEvent(0.1f);
            downEvent.AddEvent(downTimeDelay);
            OneTimerEvent scalebackReset = new OneTimerEvent(JumpReset);
            downEvent.AddEvent(scalebackReset);
            ParallelEvent jumpDownPara = new ParallelEvent();
            jumpDownPara.AddEvent(downEvent);
            ScaleEvent scaleBack = new ScaleEvent(cobj.GetRenderObjTr(), destScale, originScale, 0.4f);
            jumpDownPara.AddEvent(scaleBack);

            jumpOprEvent.AddEvent(jumpUpPara);
            jumpOprEvent.AddEvent(timeDelay);
            jumpOprEvent.AddEvent(jumpDownPara);

            jumpOprEvent.Begin();
        }
    }

   void JumpEnough()
   {
        SendMsg((int)Const_Util.UNIT_MSG.SET_STATUS, Const_Util.UnitStatus.FLY);
   }

   void JumpReset()
   {
        SendMsg((int)Const_Util.UNIT_MSG.SET_STATUS, Const_Util.UnitStatus.Normal_Move);
   }

    public override void OnBroadcastEvent(int eventID, int targetObjID, object args = null)
    {
    }

    public override void OnUpdate(float deltaTime)
    {
        if (jumpOprEvent != null)
        {
            jumpOprEvent.OnUpdate(deltaTime);
            if (jumpOprEvent.IsFinish())
            {
                jumpOprEvent = null;
            }
        }
    }

    public override void Release()
    {
        jumpOprEvent = null;
    }
}
