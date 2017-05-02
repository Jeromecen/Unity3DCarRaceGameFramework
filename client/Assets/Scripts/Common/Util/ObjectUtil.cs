using System;
using UnityEngine;
public class ObjectUtil
{
    public ObjectUtil()
    {
    }
    public static void IdentityObj(Transform tr)
    {
        tr.localScale = Vector3.one;
        tr.localPosition = Vector3.zero;
        tr.localRotation = Quaternion.identity;
    }
}

