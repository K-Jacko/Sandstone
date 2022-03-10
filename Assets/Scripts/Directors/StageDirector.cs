using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDirector : Director
{
    public GameObject Player { get; private set; }
    public static StageDirector Instance { get; private set; }
    public bool debugGrid;
    public GameObject shrine;
    public float spawnRadius;
    public int gridSize;
    public GameObject mobSpawner;
    public List<GameObject> mobSpawners;
    public int mobSpawnerYOffset;

    private Director[] _combatDirectors;
    private Pathfinding _pathfinding;
    private GameObject[] shrines;

    // Start is called before the first frame update
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

    protected override void Tick()
    {
        base.Tick();
        mobSpawners.Sort(delegate(GameObject x, GameObject y) { return Vector3.Distance(Player.transform.position,x.transform.position).CompareTo(Vector3.Distance(Player.transform.position,y.transform.position)); });
        for (int i = 0; i < mobSpawners.Count; i++)
        {
            mobSpawners[i].SetActive(false);
        }
        if(!mobSpawners[0].activeSelf)
            mobSpawners[0].SetActive(true);
        if(!mobSpawners[1].activeSelf)
            mobSpawners[1].SetActive(true);

    }

    void GenerateShrines(int noShrines)
    {
        var origin = new Vector3(0, 0, 0);
        shrines = new GameObject[noShrines];
        for (int i = 0; i < noShrines; i++)
        {
            var go = Instantiate(shrine, origin + new Vector3(Random.Range(-spawnRadius, spawnRadius),100,Random.Range(-spawnRadius, spawnRadius)), Quaternion.identity);
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
                    Debug.DrawLine(path[j] +  _pathfinding.GetGrid().GetOffset() + new Vector3(0,100,0),path[j+1] +  _pathfinding.GetGrid().GetOffset() + new Vector3(0,100,0),Color.magenta, 100f);
                    GenerateMobSpawners(path[j],path[j+1],_pathfinding);
                }
            }
        }
    }
    
    void GenerateMobSpawners(Vector3 a, Vector3 b, Pathfinding pathfinding)
    {
        var go = Instantiate(mobSpawner,a  - new Vector3(pathfinding.GetGrid().GetCellSize(),mobSpawnerYOffset,pathfinding.GetGrid().GetCellSize()) * (gridSize / 2),Quaternion.identity);
        mobSpawners.Add(go);
    }
    
 
    
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.5f,1f,0.5f,0.1f);
        Gizmos.DrawSphere(Vector3.zero, spawnRadius);
    }
    
}
