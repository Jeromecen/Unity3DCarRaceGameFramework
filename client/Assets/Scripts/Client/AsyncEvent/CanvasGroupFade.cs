using UnityEngine;

public class CanvasGroupFade : AsyncEventBase {

    CanvasGroup canvasGroup = null;
    float alphaSpeed = 0.0f;
    float mEndAlpha = 1.0f;
    public CanvasGroupFade(CanvasGroup cg, float startAlpha, float endAlpha, float time)
    {
        if (cg == null)
        {
            Debug.LogError("CanvasGroupFade cg == null");
        }
        canvasGroup = cg;
        canvasGroup.alpha = startAlpha;
        mEndAlpha = endAlpha;
        alphaSpeed = (endAlpha - startAlpha) / time;
    }

    public override void Destroy()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = mEndAlpha;
            canvasGroup = null;
        }
        End();
    }

    public override void OnUpdate(float deltaTime)
    {
        if (status == AE_Status.E_RUNNING && canvasGroup != null)
        {
            float targetAlpha = canvasGroup.alpha + alphaSpeed * deltaTime;
            canvasGroup.alpha = targetAlpha;
            if (alphaSpeed > 0 && targetAlpha >= mEndAlpha)
            {
                Destroy();
            }
            else if (alphaSpeed < 0 && targetAlpha <= mEndAlpha)
            {
                Destroy();
            }
            if (Mathf.Abs(alphaSpeed) <= Const_Util.FLT_EPSILON)
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
