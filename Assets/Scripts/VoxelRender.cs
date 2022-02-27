using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class VoxelRender : MonoBehaviour

{
    public Mesh mesh;
    public List<Vector3> verts;
    public List<int> tris;

    public float scale = 0.5f;

    public float adjustedScale;

    virtual public void Awake ()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        adjustedScale = scale * 0.5f;
    }

    void Start()
    {
        GenerateVoxelMesh( new VoxelData() );
        UpdateMesh();
    }

    virtual public void GenerateVoxelMesh(VoxelData data)
    {
        verts = new List<Vector3>();
        tris = new List<int>();
        for (int z = 0; z < data.Depth; z++)
        {
            for (int x = 0; x < data.Width; x++)
            {
                if(data.GetCell (x, z) == 0)
                {
                    continue;
                }
                MakeCube(adjustedScale, new Vector3((float)x * scale, 0,(float)z * scale));
            }
        }
    }

    public void MakeCube(float cubeScale, Vector3 cubePos)
    {
        for (int i = 0; i < 6; i++)
        {
            MakeFace(i, cubeScale, cubePos);
        }
    }

    public virtual void MakeFace(int dir, float faceScale, Vector3 facePos)
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

    virtual public void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.RecalculateNormals();
    }
}
