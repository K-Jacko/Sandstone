using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer renderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public MeshCollider collider;
    void Awake()
    {
        renderer = gameObject.GetComponent<Renderer>();
        //meshFilter = gameObject.GetComponent<MeshFilter>();
        //meshRenderer = gameObject.GetComponent<MeshRenderer>();
        //collider = gameObject.//
    }

    private void Update()
    {
        collider.sharedMesh = meshFilter.sharedMesh;
    }

    public void DrawTexture(Texture2D texture)
    {
        renderer.sharedMaterial.mainTexture = texture;
        renderer.transform.localScale = new Vector3(texture.width,1,texture.height);
    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;
    }
    
}
