using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGenerator : MapGenerator
{
    const int mapChunkSize = 239;
    
    float[,] falloffMap;
    GameObject landMesh;
    GameObject waterMesh;

    void Awake()
    {
        DrawMap();
    }
    
    public void DrawMap()
    {
        if (!landMesh)
            landMesh = CreateMesh(PrimitiveType.Plane);
        
        MapData mapData = GenerateMapData(Vector2.zero);
        MapDisplay display = landMesh.GetComponent<MapDisplay>();
        if(drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        }
        else if (drawMode == DrawMode.ColorMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColorMap(mapData.colorMap, mapChunkSize, mapChunkSize));
        }
        else if(drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, editiorPreviewLOD), TextureGenerator.TextureFromColorMap(mapData.colorMap, mapChunkSize, mapChunkSize));
            if(!waterMesh)
                DrawWater(mapData);
            gameObject.transform.localScale = new Vector3(10, 10, 10);
        }
        else if (drawMode == DrawMode.FalloffMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunkSize)));
        }
    }

    void DrawWater(MapData mapData)
    {
        waterMesh = CreateMesh(PrimitiveType.Plane);
        MapDisplay display = waterMesh.GetComponent<MapDisplay>(); 
        Material newMat = Resources.Load("Material/Water", typeof(Material)) as Material;
        waterMesh.GetComponent<MeshRenderer>().material = newMat;
        display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, 0, AnimationCurve.Constant(0,0,0), 1), Texture2D.normalTexture);
        waterMesh.transform.position = new Vector3(0, 1.3f, 0);
        
        
    }

    GameObject CreateMesh(PrimitiveType type)
    { 
        var mesh = GameObject.CreatePrimitive(type);
        mesh.transform.position = Vector3.zero;
        mesh.AddComponent<MapDisplay>().GrabComponents();
        mesh.transform.parent = gameObject.transform;
        mesh.layer = 6;
        return mesh;
    }
    
    void OnValidate()
    {
        if(lacunarity < 1)
            lacunarity = 1;
        if(octaves < 0)
            octaves = 0;
        
        falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);
    }
    
    MapData GenerateMapData(Vector2 center)
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize + 2, mapChunkSize + 2, seed, noiseScale, octaves, persistance, lacunarity, center + offset, normalizeMode);

        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];

        for (int y = 0; y < mapChunkSize; y++){
            for (int x = 0; x < mapChunkSize; x++){
                if(useFalloff)
                {
                    noiseMap[x,y] = Mathf.Clamp01(noiseMap[x,y] - falloffMap[x,y]);
                }
                float currentHeight = noiseMap[x,y];
                for (int i = 0; i < regions.Length; i++){
                    if(currentHeight >= regions [i].height)
                    {
                        colorMap[y * mapChunkSize + x] = regions[i].color;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        return new MapData(noiseMap, colorMap);
    }
    
}
