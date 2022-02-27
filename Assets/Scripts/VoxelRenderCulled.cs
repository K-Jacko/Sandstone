using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class VoxelRenderCulled : VoxelRender

{
    public GameObject prefab;

    void Awake ()
    {
        base.Awake();
    }

    void Start()
    {
        GenerateVoxelMesh( new VoxelData() );
        UpdateMesh();
    }

    override public void GenerateVoxelMesh(VoxelData data)
    {
        verts = new List<Vector3>();
        tris = new List<int>();
        for (int z = 0; z < data.Depth; z++)
        {
            for (int x = 0; x < data.Width; x++)
            {
                if(data.GetCell (x, z) == 0)
                {
                    MakePrefab(adjustedScale, new Vector3((float)x * scale, 0,(float)z * scale), data);
                    continue;
                }
                MakeCube(adjustedScale, new Vector3((float)x * scale, 0,(float)z * scale), x, z, data);
            }
        }
    }

    public void MakeCube(float cubeScale, Vector3 cubePos, int x, int z, VoxelData data)
    {
        for (int i = 0; i < 6; i++)
        {
            if(data.GetNeighbor(x, z, (Direction)i) == 0)
                MakeFace((Direction)i, cubeScale, cubePos);
        }
    }

    public void MakePrefab(float scale, Vector3 prefabPos, VoxelData data)
    {
        float yMod = gameObject.transform.position.y;
        GameObject go = Instantiate(prefab, new Vector3(prefabPos.x, prefabPos.y + yMod, prefabPos.z), new Quaternion(0,0,0,0), gameObject.transform);
        go.GetComponent<Graph>().incr = Random.Range(1, 3);
        go.transform.localScale = new Vector3(scale,scale,scale);
        
    }

    public void MakeFace(Direction dir, float faceScale, Vector3 facePos)
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

    override public void UpdateMesh()
    {
        base.UpdateMesh();
    }
}
