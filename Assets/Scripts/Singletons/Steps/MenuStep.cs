using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuStep : MonoBehaviour, ILaunchStep
{
    public event Action onComplete;
    public void Init()
    {
        SceneDirector.Instance.LoadWithLoadingScene("Menu", null);
        StartCoroutine(MenuGap());
    }

    IEnumerator MenuGap()
    {
        yield return new WaitForSeconds(3);
        onComplete?.Invoke();
    }

    public void DeInit()
    {
        var scene = SceneManager.GetSceneByName("Menu");
        SceneManager.UnloadSceneAsync(scene);
        SceneDirector.Instance.StartSession();
    }

    public bool IsRequired()
    {
        throw new NotImplementedException();
    }
}
