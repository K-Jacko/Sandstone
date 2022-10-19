using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashStep : MonoBehaviour, ILaunchStep
{
    //Splash
    public event Action OnIntroComplete;
    
    private bool isLerpingCanvas;
    private Vector3 lerpVelocity;
    private bool SoftenHorizontalFollow;


    public event Action onComplete;

    public void Init()
    {
        SceneDirector.Instance.LoadWithLoadingScene("Intro", null);
        StartCoroutine(SplashVisual());
    }

    public void DeInit()
    {
        var scene = SceneManager.GetSceneByName("Intro");
        SceneManager.UnloadSceneAsync(scene);
    }

    public bool IsRequired()
    {
        throw new NotImplementedException();
    }

    IEnumerator SplashVisual()
    {
        yield return new WaitForSeconds(3);
        onComplete?.Invoke();
    }

    void LerpToEye()
    {
        float threshHold;

        if (SoftenHorizontalFollow)
        {
            threshHold = isLerpingCanvas ? 1 : 50;
        }
        else
        {
            threshHold = 0f;
        }

        var  CanvasContainerHereY = transform.eulerAngles;
        float rdelta = Math.Abs(CanvasContainerHereY.y - Camera.main.transform.eulerAngles.y);
        if (rdelta > threshHold)
        {
            isLerpingCanvas = true;
            Vector3 newAngles = Vector3.SmoothDamp(CanvasContainerHereY,
                new Vector3(CanvasContainerHereY.x, Mathf.LerpAngle(CanvasContainerHereY.y, Camera.main.transform.eulerAngles.y, 1),
                    CanvasContainerHereY.z), ref lerpVelocity, 0.9f);

            CanvasContainerHereY = newAngles;
        }
        else
        {
            isLerpingCanvas = false;
        }
    }
    
}
