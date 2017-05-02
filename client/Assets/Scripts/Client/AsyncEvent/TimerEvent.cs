using UnityEngine;
public class TimerEvent:AsyncEventBase
{
    float time;
    public TimerEvent(float delayTime)
    {
        time = delayTime;
    }

    public override void Destroy()
    {
        End();
    }

    public override void OnUpdate(float deltaTime)
    {
        if (status == AE_Status.E_RUNNING)
        {
            time = time - deltaTime;
            if (time <= 0)
            {
                Destroy();
            }
        }
        else if (status == AE_Status.E_WAIT)
        {
            Debug.LogError("TimerEvent status is AE_Status.E_WAIT, please call begin first");
        }
    }
}

