using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class ProcedualCube : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> verts;
    List<int> tris;

    public float scale = 0.5f;
    public int posX,posY,posZ;
    private float adjustedScale;

    void Awake ()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        adjustedScale = scale * 0.5f;
    }

    void Start()
    {
        MakeCube(adjustedScale, new Vector3((float)posX * scale, (float)posY * scale, (float)posZ * scale));
        UpdateMesh();
    }

    public void MakeCube(float cubeScale, Vector3 cubePos)
    {
        verts = new List<Vector3>();
        tris = new List<int>();

        for (int i = 0; i < 6; i++)
        {
            MakeFace(i, cubeScale, cubePos);
        }
    }

    public void MakeFace(int dir, float faceScale, Vector3 facePos)
    {
        verts.AddRange(CubeMeshData.faceverts(dir, faceScale, facePos));
        int vCount = verts.Count;

        tris.Add (vCount - 4);
        tris.Add (vCount - 4 + 1);
        tris.Add (vCount - 4 + 2);
        tris.Add (vCount - 4);
        tris.Add (vCount - 4 + 2);
        tris.Add (vCount - 4 + 3);
        

    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.RecalculateNormals();
    }
}
