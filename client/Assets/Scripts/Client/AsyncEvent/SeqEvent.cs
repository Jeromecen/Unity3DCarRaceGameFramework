using System.Collections.Generic;
using UnityEngine;
public class SeqEvent:AsyncEventBase
{
    List<AsyncEventBase> seqEvent = new List<AsyncEventBase>();
    int runningIdx = 0;
    public SeqEvent()
    {
        runningIdx = 0;
    }

    public void AddEvent(AsyncEventBase refEvent)
    {
        seqEvent.Add(refEvent);
    }

    public override void Destroy()
    {
        for (int i = 0; i < seqEvent.Count; i++)
        {
            if (!seqEvent[i].IsFinish())
            {
                seqEvent[i].Destroy();
            }
        }
        End();
    }

    public override void OnUpdate(float deltaTime)
    {
        if (status == AE_Status.E_RUNNING)
        {
            if (seqEvent[runningIdx] != null && !seqEvent[runningIdx].IsFinish())
            {
                seqEvent[runningIdx].Begin();
                seqEvent[runningIdx].OnUpdate(deltaTime);
            }
            else
            {
                runningIdx = runningIdx + 1;
            }

            if (runningIdx >= seqEvent.Count)
            {
                Destroy();
            }
        }
        else if (status == AE_Status.E_WAIT)
        {
            Debug.LogError("SeqEvent status is AE_Status.E_WAIT, please call begin first");
        }
    }
}

