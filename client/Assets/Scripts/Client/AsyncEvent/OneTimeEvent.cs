using UnityEngine;
public class OneTimerEvent:AsyncEventBase
{
    public delegate void Call();
    Call callBackFunc = null;
    public OneTimerEvent(Call func)
    {
        callBackFunc = func;
    }

    public override void Destroy()
    {
        callBackFunc = null;
        End();
    }

    public override void OnUpdate(float deltaTime)
    {
        if (status == AE_Status.E_RUNNING)
        {
            if (callBackFunc != null)
            {
                callBackFunc();
                Destroy();
            }
        }
        else if (status == AE_Status.E_WAIT)
        {
            Debug.LogError("OneTimerEvent status is AE_Status.E_WAIT, please call begin first");
        }
    }
}

