using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMaker : MonoBehaviour
{
    public bool makeRock = false;
    public RockGenerator rockGenerator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(makeRock)
            SpawnRock();
    }

    public void SpawnRock()
    {
        rockGenerator.Start();
    }
}
