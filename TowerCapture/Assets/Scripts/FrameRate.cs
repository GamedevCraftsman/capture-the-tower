using UnityEngine;

public class FrameRate : MonoBehaviour
{
    int frameRateTarget = 120;
    private void Awake()
    {
        Application.targetFrameRate = frameRateTarget;
    }

}
