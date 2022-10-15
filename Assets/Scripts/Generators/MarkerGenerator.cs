using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class MarkerGenerator : MonoBehaviour
{
    private int gridSize;
    private int cellSize;
    private Vector3[] raycastLocations;
    private Grid<SpawnNode> grid;
    private List<Vector3> listForArray;
    private string path = "";

    public void Init(int gridSize, int cellSize)
    {
        this.gridSize = gridSize;
        this.cellSize = cellSize;
        MakeGrid();
        CheckForMarkerData();
        UpdateSpawnNodes();
        
    }
    
    void MakeGrid()
    {
        raycastLocations = new Vector3[gridSize * gridSize];
        grid = new Grid<SpawnNode>(gridSize, gridSize, cellSize, Vector3.zero, Color.black,
            (Grid<SpawnNode> g, int x, int y) => new SpawnNode(g,x,y), true);
    }

    void CheckForMarkerData()
    {
        if (!File.Exists(Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json"))
        {
            GenerateMarkers();
        }
        else
        {
            
            LoadMarkers();
        }
    }
    
    void GenerateMarkers()
    {
        Debug.Log("File not found : Creating anew");
        listForArray = new List<Vector3>();
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                SpawnMarkers(i,j);
            } 
        }

        for (int i = 0; i < raycastLocations.Length; i++)
        {
            raycastLocations[i] = listForArray.ToArray()[i];
            grid.SetGridObject(raycastLocations[i],new SpawnNode(raycastLocations[i]));
        }
        var raycastYData = new SpawnNodeLocations(raycastLocations);
        SaveData(raycastYData);
    }
    
    void SpawnMarkers(int i, int j)
    {
        var go = new GameObject("Marker" + $"{grid.GetWorldPosition(i, j)}");
        go.transform.position = grid.GetWorldPosition(i, j) - grid.GetOffset() / gridSize;
        go.transform.parent = gameObject.transform;
        listForArray.Add(go.transform.position);
        Debug.DrawLine(grid.GetWorldPosition(i, j)  -  grid.GetOffset() / gridSize, new Vector3(grid.GetWorldPosition(i, j).x,-100,grid.GetWorldPosition(i, j).z) -  grid.GetOffset() / gridSize, Color.red,Single.PositiveInfinity);
        Debug.Log("Spawning Markers");
    }
    
    void SaveData(SpawnNodeLocations spawnNodeLocations)
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        string savePath = path;
        string json = JsonUtility.ToJson(spawnNodeLocations);
        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);

    }

    void LoadMarkers()
    {
        Debug.Log("File Found : Loading markers");
        var data = LoadLocationData();
        for (int i = 0; i < raycastLocations.Length; i++)
        {
            raycastLocations[i] = data.SpawnLocations[i];
            var go = new GameObject("Marker" + $"{raycastLocations[i]}");
            go.transform.position = raycastLocations[i];
            go.transform.parent = gameObject.transform;
            grid.SetGridObject(raycastLocations[i],new SpawnNode(raycastLocations[i]));
            Debug.DrawLine(raycastLocations[i], new Vector3(raycastLocations[i].x,-100,raycastLocations[i].z), Color.red,Single.PositiveInfinity);
        }
    }
    
    SpawnNodeLocations LoadLocationData()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        using StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        SpawnNodeLocations data = JsonUtility.FromJson<SpawnNodeLocations>(json);
        return data;
    } 
    [InspectorButton]
    void NewSave()
    {
        for (int i = 0; i < raycastLocations.Length; i++)
        {
            raycastLocations[i] = gameObject.GetComponentsInChildren<Transform>()[i+1].position;
        }
            
        if (File.Exists(Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json"))
        {
            File.Delete(Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json");
        }
        var raycastYData = new SpawnNodeLocations(raycastLocations);
        SaveData(raycastYData);
        Debug.Log("New SpawnNodeJson Created!");
    }
    
    void UpdateSpawnNodes()
    {
        for (int i = 0; i < raycastLocations.Length; i++)
        {
            grid.SetGridObject(raycastLocations[i],new SpawnNode(raycastLocations[i], gridSize/2));
            
        }
    }

    public Vector3[] RaycastLocations()
    {
        return raycastLocations;
    }

    public Grid<SpawnNode> SpawnNodeGrid()
    {
        return grid;
    }
}
