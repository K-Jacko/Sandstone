using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public float incr = 1f;

    [SerializeField]
    Transform pointPrefab;

    [SerializeField, Range(10, 100)]
    public int resolution = 10;

    [SerializeField]
    FunctionLibrary.FunctionName function;

    public enum TransitionMode { Cycle, Random }

	[SerializeField]
	TransitionMode transitionMode;

    [SerializeField, Min(0f)]
    float functionDuration = 1f, transitionDuration = 5f;

    Transform[] points;

    float duration;

    bool transitioning;

    FunctionLibrary.FunctionName transitionFunction;

    void Awake()
    {
        float step = 2f / resolution;
        var scale = Vector3.one * step;
        points = new Transform[resolution*resolution];
        for (int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
            points[i] = point;
        }
    }

    void Update()
    {   
        duration += Time.deltaTime;
        if(transitioning)
        {
           	if (duration >= transitionDuration) {
				duration -= transitionDuration;
				transitioning = false;
			}
        }
        else if(duration >= functionDuration)
        {
            transitioning = true;
            transitionFunction = function;
            PickNextFunction();
        }

        if(transitioning)
        {
            UpdateFunctionTransition();
        }
        else
        {
          UpdateFunction();  
        }
        
    }

    void PickNextFunction()
    {
        function = transitionMode == TransitionMode.Cycle ?
		FunctionLibrary.GetNextFunctionName(function) :
		FunctionLibrary.GetRandomFunctionNameOtherThan(function);
    }

    void UpdateFunction()
    {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
        float time = Time.time;
		float step = 2f / resolution;
        float v = 0.5f * step - 1f;
		for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
			if (x == resolution) 
            {
				x = 0;
				z += 1;
                v = (z + 0.05f) * step - 1f;
			}
			float u = (x + 0.5f) * step - 1f;
			points[i].localPosition = f(u, v, time, incr);
		}
    }
    void UpdateFunctionTransition()
    {
        FunctionLibrary.Function from = FunctionLibrary.GetFunction(transitionFunction), to = FunctionLibrary.GetFunction(function);
        float progress = duration / transitionDuration;
        float time = Time.time;
		float step = 2f / resolution;
        float v = 0.5f * step - 1f;
		for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
			if (x == resolution) 
            {
				x = 0;
				z += 1;
                v = (z + 0.05f) * step - 1f;
			}
			float u = (x + 0.5f) * step - 1f;
			points[i].localPosition = FunctionLibrary.Morph(u, v, time, incr, from, to, progress);
		}
    }
}
