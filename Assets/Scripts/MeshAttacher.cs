using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshCollider),typeof(VoxelRender))]
public class MeshAttacher : MonoBehaviour
{
    public MeshCollider collider;
    public MeshFilter meshFilter;
    
    public Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();

    }

    // Update is called once per frame
    void Update()
    {

            collider = gameObject.GetComponent<MeshCollider>();
            collider.sharedMesh = gameObject.GetComponent<VoxelRender>().mesh;
    }
}
