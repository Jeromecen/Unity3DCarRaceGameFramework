using UnityEngine;

public class MoveToEventY : AsyncEventBase {

    Transform u3dTr;
    Vector3 targetPos;
    Vector3 speed;
    float duration;
    public MoveToEventY(Transform moveTr, Vector3 destPos, float time)
    {
        u3dTr = moveTr;
        targetPos = destPos;
        duration = time;
        speed = (destPos - u3dTr.localPosition) / duration;
    }

    public override void Destroy()
    {
        if (u3dTr != null)
        {
            u3dTr.localPosition = targetPos;
            u3dTr = null;
        }
        End();
    }

    void CheckSetXPosValid()
    {
        if (u3dTr.localPosition.x > targetPos.x || u3dTr.localPosition.x < targetPos.x)
        {
            u3dTr.localPosition = new Vector3( targetPos.x, u3dTr.localPosition.y, u3dTr.localPosition.z);
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        if (status == AE_Status.E_RUNNING && u3dTr != null)
        {
            u3dTr.localPosition = u3dTr.localPosition + speed * deltaTime;
            CheckSetXPosValid();
            if ((speed.y >= 0 && u3dTr.localPosition.y >= targetPos.y))
            {
                u3dTr.localPosition = new Vector3(targetPos.x, u3dTr.localPosition.y, u3dTr.localPosition.z);
                Destroy();
            }
            else if ((speed.y <= 0 && u3dTr.localPosition.y <= targetPos.y))
            {
                u3dTr.localPosition = new Vector3(targetPos.x, u3dTr.localPosition.y, u3dTr.localPosition.z);
                Destroy();
            }

            if (Mathf.Abs(speed.x) <= Const_Util.FLT_EPSILON && Mathf.Abs(speed.y) <= Const_Util.FLT_EPSILON && Mathf.Abs(speed.y) <= Const_Util.FLT_EPSILON)
            {
                Destroy();
            }
        }
        else if (status == AE_Status.E_WAIT)
        {
            Debug.LogError("MoveToEvent status is AE_Status.E_WAIT, please call begin first");
        }
    }
}
