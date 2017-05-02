using UnityEngine;

public class AccMoveEvent : AsyncEventBase {

    Transform u3dTr;
    Vector3 targetPos;
    Vector3 speed;
    Vector3 acc;
    public AccMoveEvent(Transform moveTr, Vector3 destPos, float time, bool isAcc)
    {
        u3dTr = moveTr;
        targetPos = destPos;
//        duration = time;
        float timeExp = time * time;
        Vector3 distanceDouble = 2 * (destPos - moveTr.localPosition);
        acc = distanceDouble / timeExp; //new Vector3(distanceDouble.x / timeExp, distanceDouble.y / timeExp, 0.0f);
        // 加速
        if (isAcc)
        {
            speed = Vector3.zero;
        }
        // 减速
        else
        {
            speed = new Vector3(acc.x * time, acc.y * time, 0.0f);
            acc = -acc;
        }
    }

    public override void Destroy()
    {
        if (u3dTr != null)
        {
            u3dTr.localPosition = targetPos;//new Vector3(targetPos.x, u3dTr.localPosition.y, u3dTr.localPosition.z);
            u3dTr = null;
        }
        End();
    }

    public override void OnUpdate(float deltaTime)
    {
        if (status == AE_Status.E_RUNNING && u3dTr != null)
        {
            Vector3 curAddSpeed = acc * deltaTime;
            speed += curAddSpeed;
            Vector3 addDistance = speed * deltaTime;
            u3dTr.localPosition = new Vector3(u3dTr.localPosition.x + addDistance.x, u3dTr.localPosition.y + addDistance.y, 0.0f);
            if ((speed.x >= 0 && u3dTr.localPosition.x >= targetPos.x) || (speed.y >= 0 && u3dTr.localPosition.y >= targetPos.y))
            {
//                u3dTr.localPosition = new Vector3(targetPos.x, u3dTr.localPosition.y, u3dTr.localPosition.z);
                Destroy();
            }
            else if ((speed.x <= 0 && u3dTr.localPosition.x <= targetPos.x)|| (speed.y <= 0 && u3dTr.localPosition.y <= targetPos.y))
            {
//                u3dTr.localPosition = new Vector3(targetPos.x, u3dTr.localPosition.y, u3dTr.localPosition.z);
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
