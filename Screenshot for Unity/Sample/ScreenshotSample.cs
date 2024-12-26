using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Screenshot;

public class ScreenshotSample : MonoBehaviour
{
    void Update()
    {
        ScreenShotCore.CheckForScreenshotKey();
    }
}
