using UnityEngine;
using UnityEngine.UI;
public class UIAccMoveEvent : AsyncEventBase {

    RectTransform u3dTr;
    Vector3 targetPos;
    Vector3 speed;
    Vector3 acc;
    public UIAccMoveEvent(RectTransform moveTr, Vector3 destPos, float time, bool isAcc)
    {
        u3dTr = moveTr;
        targetPos = destPos;
        float timeExp = time * time;
        Vector3 distanceDouble = 2 * (destPos - moveTr.anchoredPosition3D);
        acc = distanceDouble / timeExp;
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
            u3dTr.anchoredPosition3D = targetPos;
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
            u3dTr.anchoredPosition3D += speed * deltaTime;;
    
            if ((speed.x >= 0 && u3dTr.anchoredPosition3D.x >= targetPos.x) && (speed.y >= 0 && u3dTr.anchoredPosition3D.y >= targetPos.y))
            {
                Destroy();
            }
            else if ((speed.x <= 0 && u3dTr.anchoredPosition3D.x <= targetPos.x)&& (speed.y <= 0 && u3dTr.anchoredPosition3D.y <= targetPos.y))
            {
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
