using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDirector : MonoBehaviour
{
    public static SceneDirector Instance { get; private set; }
    
    public event Action OnLoadingStart;
    private Scene activeScene;
    private void Start()
    {
        Instance = this;
        var launchSystem = gameObject.AddComponent<LaunchSystem>();
        launchSystem.Init();
    }
    
    public void LoadScene(String name, Action callback = null)
    {
        SceneManager.LoadScene(name,LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
        callback?.Invoke();
    }

    public void UnloadScene(String name)
    {
        SceneManager.UnloadSceneAsync(name);
    }
    
    public void LoadLoading(String nextScene, Action callback)
    {
        Instance.LoadScene("Loading", () =>
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
            // PreLoad
            //Change Culling
            //MovePlayer
            //
            //PostLoad On player moved and animations played
            
            StartCoroutine(TempVisualDelay());
        });
        LoadScene(nextScene);
        callback?.Invoke();
    }
    
    private IEnumerator TempVisualDelay()
    {
        yield return new WaitForSeconds(3);
        Instance.UnloadLoading();
    }
    
    public void UnloadLoading()
    {
        Instance.UnloadScene("Loading");
    }

    
}