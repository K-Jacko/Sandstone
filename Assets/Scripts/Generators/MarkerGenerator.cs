using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;

public class MarkerGenerator : MonoBehaviour
{
    private int gridSize;
    private int cellSize;
    private Vector3[] raycastLocations;
    private Grid<SpawnNode> grid;
    private List<Vector3> listForArray;
    private string path = "";

    public void Init(int gridSize, int cellSize, Action callback)
    {
        this.gridSize = gridSize;
        this.cellSize = cellSize;
        MakeGrid();
        LoadMarkers();
        callback?.Invoke();
    }
    
    void MakeGrid()
    {
        raycastLocations = new Vector3[gridSize * gridSize];
        grid = new Grid<SpawnNode>(gridSize, gridSize, cellSize, Vector3.zero, Color.black,
            (Grid<SpawnNode> g, int x, int y) => new SpawnNode(g,x,y), true);
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

    void LoadMarkers()
    {
        var data = SceneDirector.Instance.currentMapData;
        for (int i = 0; i < raycastLocations.Length; i++)
        {
            raycastLocations[i] = data.spawnLocations[i];
            var go = new GameObject("Marker" + $"{raycastLocations[i]}");
            go.transform.position = raycastLocations[i];
            go.transform.parent = gameObject.transform;
            grid.SetGridObject(raycastLocations[i],new SpawnNode(raycastLocations[i]));
            Debug.DrawLine(raycastLocations[i], new Vector3(raycastLocations[i].x,-100,raycastLocations[i].z), Color.red,Single.PositiveInfinity);
        }
        UpdateSpawnNodes();
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
