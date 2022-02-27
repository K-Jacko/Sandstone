using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class _MeshGenerator : MonoBehaviour
{

    public Mesh mesh;

    public Vector3[] verts;
    public int[] tris;

    public void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    public void Update()
    {
        MakeMeshData();
        CreateMesh();

    }

    public void MakeMeshData()
    {
        verts = new Vector3[]
        {
            new Vector3(0,YValue.instance.yValue,0),
            new Vector3(0,0,1),
            new Vector3(1,0,0),
            new Vector3(1,0,1)
        };
        tris = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3

        };
    }

    public void CreateMesh()
    {
        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;

        mesh.RecalculateNormals();

    }
}
