using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class LaunchSystem : MonoBehaviour
{
    public static LaunchSystem Instance { get; private set; }
    private readonly Dictionary<string, Type> allLaunchSteps = new Dictionary<string, Type>();
    private readonly IList<ILaunchStep> launchSteps = new List<ILaunchStep>();
    private int index;
    private Stopwatch stopwatch;

    public void Init()
    {
        Instance = this;
        index = 0;
        allLaunchSteps.Add("SplashIntro", typeof(IntroStep));
        
        var tempSchema = GetSchema(new CoreSchema());
        LoadSchema(tempSchema);
        stopwatch = new Stopwatch();
        LaunchSequence();
    }

    CoreSchema GetSchema(CoreSchema data)
    {
        //Grab Schema from text file later to be able to control launch sequence remotely
        Array.Resize(ref data.schema,1);
        data.schema[0] = new string("SplashIntro");
        return data;
    }

    void LoadSchema(CoreSchema data)
    {
        foreach (string stage in data.schema)
        {
            var launchStep = (ILaunchStep)gameObject.AddComponent(allLaunchSteps["SplashIntro"]);
            if(!launchSteps.Contains(launchStep))
                launchSteps.Add(launchStep);
        }
        
    }

    private void LaunchSequence()
    {
        
        launchSteps[index].onComplete += OnStageComplete;
        launchSteps[index].Init();
    }

    void OnStageComplete()
    {
        launchSteps[index].onComplete -= OnStageComplete;
        Debug.Log("Step Complete: " + launchSteps[index].GetType() + "Elapsed time ms: " + stopwatch.ElapsedMilliseconds);
        launchSteps[index].DeInit();
        Next();
    }

    void Next()
    {
        if (index >= launchSteps.Count - 1)
        {
            return;
        }
        index++;
        LaunchSequence();
    }
    
}

public class CoreSchema
{
    public string[] schema;
}