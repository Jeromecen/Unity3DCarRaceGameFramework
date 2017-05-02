using System;
using UnityEngine;
public class ParticleUtil
{
    public ParticleUtil()
    {
    }

    public static float GetParticleLength(Transform particleTr)
    {
        if (particleTr == null)
        {
            return 0.0f;
        }
        ParticleSystem [] allPS = particleTr.GetComponentsInChildren<ParticleSystem>();
        if (allPS == null)
            return 0;
        float maxDuration = 0;
        for (int i = 0; i < allPS.Length; i++)
        {
            ParticleSystem ps = allPS[i];
            if (ps.emission.enabled)
            {
                if (ps.main.loop)
                {
                    return -1f;
                }
                float dunration = ps.main.startDelay.constant + ps.main.duration + ps.main.startLifetime.constant;
                if (dunration > maxDuration)
                {
                    maxDuration = dunration;
                }
            }
        }

        return maxDuration;
    }
}

