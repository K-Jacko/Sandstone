using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeScreenshot : MonoBehaviour
{
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Space))
            ScreenCapture.CaptureScreenshot("Journey Screenshot.png");
    }
}
