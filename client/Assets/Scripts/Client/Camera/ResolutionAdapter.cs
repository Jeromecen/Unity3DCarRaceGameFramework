using UnityEngine;
using System.Collections;

public class ResolutionAdapter : MonoBehaviour
{
    public float dev16Over9Size = 6.4f;
    public float dev4Over3Size = 4.8f;

    public float dev16Over9MapBottomOffset = 0.0f;
    public float dev4Over3MapBottomOffset = -1.62f;

    void Awake()
    {

        Camera.main.orthographicSize = UILogic.GetLinearInterpForScreen(dev16Over9Size, dev4Over3Size, true);
        float cameraOffsetForMapBottom = UILogic.GetLinearInterpForScreen(dev16Over9MapBottomOffset, dev4Over3MapBottomOffset, true);
        Camera.main.transform.localPosition = new Vector3(0, cameraOffsetForMapBottom, 0);
        BattleInstance.instance.BaseStartpoint = cameraOffsetForMapBottom;
    }

    // Update is called once per frame
    //	void Update () {
    //	}
}