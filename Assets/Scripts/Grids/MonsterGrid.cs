using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterGrid : Grid
{
    public int scale = 10;
    public float gizmoRadius = 0.1f;
    public List<Vector3> MonsterPositions;
    public GameObject monsterSpawner;
    
    
    private void Awake()
    {
        //GenerateData();
        GenerateGrid( new GridData());
    }
    
    void GenerateGrid(GridData data)
    {
        for (int i = 0; i < data.Depth; i++)
        {
            for (int j = 0; j < data.Width; j++)
            {
                if (data.GetCell(i, j) == 0)
                {
                    GenerateSpawner(i, j);
                    //continue;
                }
                GeneratePoint(i * scale, j * scale);
            }
        }
    }

    void GenerateSpawner(int x, int z)
    {
        Instantiate(monsterSpawner, new Vector3(x * scale, 20, z * scale), new Quaternion(0, 0, 0, 0));
    }
    void GeneratePoint(int x, int z)
    {
        MonsterPositions.Add(new Vector3(x,20,z));
    }

    public void Update()
    {
        print($"{rng()}");
    }

    int rng()
    {
        return Random.Range(0, 1);
    }
    void OnDrawGizmos()
    {
        // Gizmos.color = Color.black;
        // for (int i = 0; i <= MonsterPositions.Capacity; i++)
        // {
        //     Gizmos.DrawSphere(MonsterPositions[i], gizmoRadius);
        // }
    }

    [InspectorButton]
    void GenerateGrid()
    {
        GenerateGrid(new GridData());
    }
    

}
