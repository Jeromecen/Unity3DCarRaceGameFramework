using UnityEngine;

public class MoveByEvent : AsyncEventBase {
    Transform u3dTr;
    Vector3 movePos;
    Vector3 speed;
    Vector3 totalMove = Vector3.zero;
    float duration;
    public MoveByEvent(Transform moveTr, Vector3 moveDistance, float time)
    {
        u3dTr = moveTr;
        movePos = moveDistance;
        duration = time;
        speed = movePos / duration;
    }

    public override void Destroy()
    {
        u3dTr = null;
        End();
    }

    public override void OnUpdate(float deltaTime)
    {
        if (status == AE_Status.E_RUNNING && u3dTr != null)
        {
            Vector3 curFrameMove = speed * deltaTime;
            u3dTr.localPosition = u3dTr.localPosition + curFrameMove;
            totalMove = totalMove + curFrameMove;
            if (speed.x > 0 && totalMove.x >= movePos.x)
            {
                Destroy();
            }
            else if (speed.x < 0 && totalMove.x <= movePos.x)
            {
                Destroy();
            }
        }
        else if (status == AE_Status.E_WAIT)
        {
            Debug.LogError("MoveByEvent status is AE_Status.E_WAIT, please call begin first");
        }
    }
	
}
