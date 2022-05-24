using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor (typeof(IslandGenerator))]
public class IslandGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        IslandGenerator mapGen = (IslandGenerator)target;

        if(DrawDefaultInspector())
            if(mapGen.autoUpdate)
                mapGen.DrawMap();

        if(GUILayout.Button ("Generate"))
        {
            mapGen.DrawMap();
        };
    }
}