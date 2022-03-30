using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class StageDirector : Director
{
    public Player Player { get; private set; }
    public static StageDirector Instance { get; private set; }
    public float coEef;
    public bool debugGrid;
    public GameObject shrine;
    public float shrineSpawnRadius;
    public int gridSize;
    public List<GameObject> safeZones;
    public GameObject[] shrines;

    private Director[] _combatDirectors;
    private Pathfinding _pathfinding;
    public NavMeshSurface navMesh;

    // Start is called before the first frame update
    //Use Task action to make sure Everything load is right order
    public override void Awake()
    {
        //Task action init
        base.Awake();
        LoadMonsters();
        Init();
        GenerateShrines(4);
        GeneratePaths(shrines);
        LoadZiggurat();
    }

    void Init()
    {
        InfinateTerrain.OnMapUpdated += GenerateNavMesh;
        Player = GameObject.Find("Player").GetComponent<Player>();
        Instance = this;
        var MapGenerator = GetComponent<InfinateTerrain>();
        MapGenerator.Init();
        _combatDirectors = GetDirectors();
    }

    Director[] GetDirectors()
    {
        var combatDirectors = FindObjectsOfType<Director>();
        foreach (var combatDirector in combatDirectors)
        {
            combatDirector.Init();
        }

        return combatDirectors;
    }
    void GenerateShrines(int noShrines)
    {
        var origin = new Vector3(0, 0, 0);
        shrines = new GameObject[noShrines];
        for (int i = 0; i < noShrines; i++)
        {
            var go = Instantiate(shrine, origin + new Vector3(Random.Range(-shrineSpawnRadius, shrineSpawnRadius),0,Random.Range(-shrineSpawnRadius, shrineSpawnRadius)), Quaternion.identity);
            shrines[i] = go;
        }
    }
    void GeneratePaths(GameObject[] _shrines)
    {
        for (int i = 0; i < _shrines.Length; i++)
        {
            _pathfinding = new Pathfinding(gridSize, gridSize, debugGrid);
            
            List<Vector3> path = _pathfinding.FindPath(new Vector3(0,0,0), _shrines[i].transform.position);
            if (path != null)
            {
                for (int j = 0; j < path.Count - 1; j++)
                {
                    Debug.DrawLine(path[j] +  _pathfinding.GetGrid().GetOffset() + new Vector3(0,50,0),path[j+1] +  _pathfinding.GetGrid().GetOffset() + new Vector3(0,50,0),Color.red, 100f);
                }
            }
        }
    }
    void LoadZiggurat()
    {
        var zig = Resources.Load("Prefabs/Ziggurat");
        Instantiate(zig, new Vector3(0,200,0), quaternion.identity);
        safeZones.Add(zig.GameObject());
    }
    
    public void GenerateNavMesh()
    {
        if (!navMesh)
        {
            navMesh = gameObject.AddComponent<NavMeshSurface>();
            int layerMask = 1 << 6;
            navMesh.layerMask = layerMask;
            navMesh.collectObjects = CollectObjects.Volume;
            navMesh.size = new Vector3(300, 30, 300);
            navMesh.center = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
            navMesh.BuildNavMesh();
        }
        else
        {
            navMesh.center = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
            navMesh.UpdateNavMesh(navMesh.navMeshData);
        }

    }

    private void OnDisable()
    {
        TimeManager.OnMinChanged -= GenerateNavMesh;
    }
}
