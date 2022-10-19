using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    public string path;
    public Vector3[] spawnLocations;
    public String[] validMobs;
    
    public MapData(Vector3[] spawnLocations)
    {
        this.spawnLocations = spawnLocations;
    }

    public MapData(Vector3[] spawnLocations, String[] validMobs)
    {
        this.spawnLocations = spawnLocations;
        this.validMobs = validMobs;
    }
}
