using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    public int width = 300;
    public int height = 300;

    public float scale = 20f;

    private Renderer renderer;

    private Texture iChannel1;
    // Start is called before the first frame update
    void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        iChannel1 = renderer.material.GetTexture("iChannel1");
    }

    // Update is called once per frame
    void Update()
    {
        if(renderer.material.mainTexture == null)
            renderer.material.mainTexture = GenerateTexture();
        
        if(iChannel1 != null)
            iChannel1 = renderer.material.mainTexture;
        
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }

    Color CalculateColor (int x, int y)
    {
        float xCoord = (float)x / width * scale;
        float yCoord = (float)y / height * scale;
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }
}
