using UnityEngine;

public class ScaleEvent : AsyncEventBase {
    Transform scaleNodeTr;
    Vector3 scaleSize;
    Vector3 scaleSpeed;
    public ScaleEvent(Transform scaleTr, Vector3 scaleSrc, Vector3 scaleDest, float time)
    {
        
        scaleNodeTr = scaleTr;
        scaleSize = scaleDest;
        Vector3 diffScale = scaleSize - scaleSrc;
        scaleSpeed = diffScale / time;
    }

    public override void Destroy()
    {
        if (scaleNodeTr != null)
        {
            scaleNodeTr.localScale = scaleSize;
            End();
            scaleNodeTr = null;
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        if (status == AE_Status.E_RUNNING)
        {
            scaleNodeTr.localScale = scaleNodeTr.localScale + scaleSpeed * deltaTime;
            if (scaleSpeed.x > 0 && scaleNodeTr.localScale.x >= scaleSize.x)
            {
                scaleNodeTr.localScale = scaleSize;
                Destroy();
            }
            else if (scaleSpeed.x < 0 && scaleNodeTr.localScale.x <= scaleSize.x)
            {
                scaleNodeTr.localScale = scaleSize;
                Destroy();

            }
        }
        else if (status == AE_Status.E_WAIT)
        {
            Debug.LogError("ScaleEvent status is AE_Status.E_WAIT, please call begin first");
        }
    }
}

