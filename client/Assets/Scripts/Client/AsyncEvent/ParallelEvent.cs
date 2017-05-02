using System.Collections.Generic;
using UnityEngine;
public class ParallelEvent:AsyncEventBase
{
    int finishCount = 0;
    List<AsyncEventBase> parallelEvent = new List<AsyncEventBase>();
    public ParallelEvent()
    {
    }
    public void AddEvent(AsyncEventBase eventRef)
    {
        parallelEvent.Add(eventRef);
    }

    public override void Destroy()
    {
        for (int i = 0; i < parallelEvent.Count; i++)
        {
            if (!parallelEvent[i].IsFinish())
            {
                parallelEvent[i].Destroy();
            }
        }
        End();
    }

    public override void Begin()
    {
        status = AE_Status.E_RUNNING;
        for (int i = 0; i < parallelEvent.Count; i++)
        {
            if (!parallelEvent[i].IsFinish())
            {
                parallelEvent[i].Begin();
            }
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        if (status == AE_Status.E_RUNNING)
        {
            finishCount = 0;
            for (int i = 0; i < parallelEvent.Count; i++)
            {
                if (!parallelEvent[i].IsFinish())
                {
                    parallelEvent[i].OnUpdate(deltaTime);
                }
                else
                {
                    finishCount++;
                }
            }

            if (finishCount == parallelEvent.Count)
            {
                Destroy();
            }
        }
        else if (status == AE_Status.E_WAIT)
        {
            Debug.LogError("ParallelEvent status is AE_Status.E_WAIT, please call begin first");
        }
    }
}
