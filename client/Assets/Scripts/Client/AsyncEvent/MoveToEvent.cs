using UnityEngine;

public class MoveToEvent : AsyncEventBase {

    Transform u3dTr;
    Vector3 targetPos;
    Vector3 speed;
    float duration;
    public MoveToEvent(Transform moveTr, Vector3 destPos, float time)
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
            u3dTr.localPosition = new Vector3(targetPos.x, u3dTr.localPosition.y, u3dTr.localPosition.z);
            u3dTr = null;
        }
        End();
    }

    public override void OnUpdate(float deltaTime)
    {
        if (status == AE_Status.E_RUNNING && u3dTr != null)
        {
            u3dTr.localPosition = u3dTr.localPosition + speed * deltaTime;
            if ((speed.x >= 0 && u3dTr.localPosition.x >= targetPos.x))// || (speed.y >= 0 && u3dTr.localPosition.y >= targetPos.y))
            {
                u3dTr.localPosition = new Vector3(targetPos.x, u3dTr.localPosition.y, u3dTr.localPosition.z);
                Destroy();
            }
            else if ((speed.x <= 0 && u3dTr.localPosition.x <= targetPos.x))// || (speed.y <= 0 && u3dTr.localPosition.y <= targetPos.y))
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
