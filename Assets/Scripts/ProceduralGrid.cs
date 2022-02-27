using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class ProceduralGrid : MonoBehaviour
{
    Mesh mesh;
    Vector3[] verts;
    int[] tris;

    public float cellsize = 1;
    public Vector3 gridOffset;
    public int gridSize;
    public int customY;
    public bool OnUpdate;

    void Awake ()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    void Start()
    {
        MakeContiguousProceduralGrid();
        //MakeDiscreteProceduralGrid();
        UpdateMesh();
    }

    void Update()
    {
        if(OnUpdate == true)
        {
            UpdateMesh();
            Debug.Log("off");
            OnUpdate = false;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
    }

//Seperate Verts : Steep
    void MakeDiscreteProceduralGrid()
    {
        verts = new Vector3[gridSize * gridSize * 4];
        tris = new int[gridSize * gridSize * 6];

        int v = 0;
        int t = 0;

        float vertOffset = cellsize * 0.5f;

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Vector3 cellOffset = new Vector3 (i * cellsize, 0, j * cellsize);
                verts[v] = new Vector3(-vertOffset, 0, -vertOffset) +gridOffset +cellOffset;
                verts[v+1] = new Vector3(-vertOffset, 0, vertOffset) +gridOffset +cellOffset;
                verts[v+2] = new Vector3(vertOffset,0,-vertOffset) +gridOffset +cellOffset;
                verts[v+3] = new Vector3(vertOffset,0,vertOffset) +gridOffset +cellOffset;

                tris[t] = v;
                tris[t+1] = tris[t+4] = v+1;
                tris[t+2] = tris[t+3] = v+2;
                tris[t+5] = v+3;

                v += 4;
                t += 6;
            }
        }


    }

    //Shared Verts : Smooth 
    void MakeContiguousProceduralGrid()
    {
        verts = new Vector3[(gridSize + 1) * (gridSize + 1)];
        tris = new int[gridSize * gridSize * 6];

        int v = 0;
        int t = 0;
    
        float vertOffset = cellsize * 0.5f;
        

        for (int i = 0; i <= gridSize; i++){
            for (int j = 0; j <= gridSize; j++)
            {
                verts[v] = new Vector3 ((i * cellsize) - vertOffset,0, (j * cellsize) - vertOffset);
                v++;
            }
        }

        v = 0;

        for (int i = 0; i < gridSize; i++){
            for (int j = 0; j < gridSize; j++)
            {
                tris[t] = v;
                tris[t+1] = tris[t+4] = v+1;
                tris[t+2] = tris[t+3] = v + (gridSize +1);
                tris[t+5] = v + (gridSize + 1) + 1;
                v++;
                t += 6;
            }
            v++;

        }
    }
}
