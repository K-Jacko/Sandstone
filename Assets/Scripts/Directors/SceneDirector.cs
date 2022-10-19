using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class SceneDirector : MonoBehaviour
{
    
    // This now will be responsible for loading the maps data and organizing the mobCards and propCards for Directors 
    public static SceneDirector Instance { get; private set; }
    public MapData currentMapData;

    public Entity Player;
    public Stopwatch TimeInGame;
    public int coEef = 1;
    
    private void Start()
    {
        Instance = this;
        Player = GameObject.Find("Player").GetComponent<Player>();
        var launchSystem = gameObject.AddComponent<LaunchSystem>();
        TimeInGame = new Stopwatch();
        launchSystem.Init();
    }

    void GetMapData(Scene arg0, LoadSceneMode loadSceneMode)
    {
        var path = Application.dataPath + Path.AltDirectorySeparatorChar + "Scripts" +
                   Path.AltDirectorySeparatorChar + "Data" + Path.AltDirectorySeparatorChar + $"{SceneManager.GetSceneAt(2).name}" + ".json";
        if (File.Exists(path))
        {
            using StreamReader reader = new StreamReader(path);
            string json = reader.ReadToEnd();
            MapData data = JsonUtility.FromJson<MapData>(json);
            currentMapData = data;
            currentMapData.path = path;
        }
        else
        {
            Debug.Log("MapData for" + $"{SceneManager.GetSceneAt(2).name}" + "Does Not Exist");
            GenerateMapData();
            
        }
        TimeInGame.Start();
    }

    void GenerateMapData()
    {
        //Get Vector3[] from MarkerGenerator
        //Get MobCard[] from Dictionary
        //Both will come from stageDirector
        //Path set on load

        string savePath = Application.dataPath + Path.AltDirectorySeparatorChar + "Scripts" +
                          Path.AltDirectorySeparatorChar + "Data" + Path.AltDirectorySeparatorChar + $"{SceneManager.GetSceneAt(1)}" + ".json";;
        string json = JsonUtility.ToJson(new MapData(StageDirector.markerGenerator.RaycastLocations(), Dictionaries.GetValidMobCards(new int[]{0,1})),true);
        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }

    public void StartSession()
    {
        //Get MapRoster From Dict
        SceneManager.sceneLoaded +=  GetMapData;
        Instance.LoadWithLoadingScene("Test Stage",null);
        
    }

    public void LoadWithLoadingScene(String nextScene, Action callback)
    {
        SceneManager.LoadScene("Loading",LoadSceneMode.Additive);
        StartCoroutine(LoadingGap());
        SceneManager.LoadScene(nextScene,LoadSceneMode.Additive);
        callback?.Invoke();
    }
    
    private IEnumerator LoadingGap()
    {
        //PutVisualHere
        yield return new WaitForSeconds(1);
        Instance.UnloadLoading();
    }
    
    public void UnloadLoading()
    {
        SceneManager.UnloadSceneAsync("Loading");
    }

    private void Update()
    {
        coEef = ((int)TimeInGame.Elapsed.TotalSeconds / 60);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -=  GetMapData;
    }
}