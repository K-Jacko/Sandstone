using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class StageDirector : Director
{
    public GameObject Player { get; private set; }
    public static StageDirector Instance { get; private set; }
    public float coEef;
    public bool debugGrid;
    public GameObject shrine;
    public float shrineSpawnRadius;
    public int gridSize;

    private Director[] _combatDirectors;
    private Pathfinding _pathfinding;
    private GameObject[] shrines;
    

    // Start is called before the first frame update
    //Use Task action to make sure Everything load is right order
    public override void Awake()
    {
        base.Awake();
        Player = GameObject.Find("Player");
        Instance = this;
        LoadMonsters();
        GenerateShrines(4);
        GeneratePaths(shrines);
        
        
        //Collect directors better
        _combatDirectors = FindObjectsOfType<Director>();
        foreach (var combatDirector in _combatDirectors)
        {
            combatDirector.Init();
        }
    }
    

    void GenerateShrines(int noShrines)
    {
        var origin = new Vector3(0, 0, 0);
        shrines = new GameObject[noShrines];
        for (int i = 0; i < noShrines; i++)
        {
            var go = Instantiate(shrine, origin + new Vector3(Random.Range(-shrineSpawnRadius, shrineSpawnRadius),100,Random.Range(-shrineSpawnRadius, shrineSpawnRadius)), Quaternion.identity);
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
    

    
}
