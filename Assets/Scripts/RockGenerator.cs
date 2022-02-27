using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class RockGenerator : MonoBehaviour 
{
 
    public int seed = 37823787;
    private Material mat;
    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector3> doneVerts = new List<Vector3>();
    private Vector3 center;
    public int vertCount;
    
 
    public void Start()
    {
        Random.InitState(seed);
        float offset = Random.Range(0, 20);
 
        Mesh mesh = GetComponent<MeshFilter>().mesh;
    
        for (int s = 0; s < mesh.vertices.Length; s ++)
        {
            vertices.Add(mesh.vertices[s]);
        }
 
        center = GetComponent<Renderer>().bounds.center;
 
        for (int v = 0; v < vertices.Count; v++)
        {
            bool used = false;
            for (int k = 0; k < doneVerts.Count; k++)
            {
                if (doneVerts[k] == vertices[v])
                {
                    used = true;
                }
            }
            if (!used)
            {
                Vector3 curVector = vertices[v];
                doneVerts.Add(curVector);
                int smoothing = Random.Range(4, 6);
                Vector3 changedVector = (curVector + ((curVector - center) * (Mathf.PerlinNoise((float) v / offset, (float)v / offset)/smoothing)));
                for (int s = 0; s < vertices.Count; s ++)
                {
                    if (vertices[s] == curVector)
                    {
                        vertices[s] = changedVector;
                    }
                }
            }
        }
 
        mesh.SetVertices(vertices);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        
    }

    void update()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        vertCount = mesh.vertices.Length;
    }
}
 
 