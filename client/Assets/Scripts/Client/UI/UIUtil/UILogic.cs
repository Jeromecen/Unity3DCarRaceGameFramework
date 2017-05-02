using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogic
{
    const float M16OVER9 = 1.777777f;
    const float M4OVER3 = 1.333333f;

    public static float GetLinearInterpForScreen(float size16over9, float size4over3, bool isVertical)
    {
        float internalValue = size16over9 - size4over3;
        float curScreenWidth = Screen.width;
        float curScreenHeight = Screen.height;
        float curSize = 0;
        if (isVertical)
        {
            curSize = curScreenHeight / curScreenWidth;
        }
        else
        {
            curSize = curScreenWidth / curScreenHeight;
        }
        float deltaValue = (curSize - M4OVER3) / (M16OVER9 - M4OVER3);
        return size4over3 + deltaValue * internalValue;
    }
}
