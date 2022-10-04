using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroStep : MonoBehaviour, ILaunchStep
{
    
    public bool SoftenHorizontalFollow;
    public bool Introbool;
    public event Action OnIntroComplete;
    
    private bool isLerpingCanvas;
    private Vector3 lerpVelocity;


    public event Action onComplete;

    public void Init()
    {
        LoadIntroScene(() =>
        {
            onComplete?.Invoke();
        });
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

    private void Update()
    {
        LerpToEye();
        if (Introbool)
        {
            SceneDirector.Instance.LoadLoading("Menu",SceneDirector.Instance.UnloadLoading);
            Introbool = false;
        }
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

    void LoadIntroScene(Action callback)
    {
        SceneDirector.Instance.LoadScene("intro");
        callback?.Invoke();
    }
    void UnloadScene()
    {
        
    }
}
