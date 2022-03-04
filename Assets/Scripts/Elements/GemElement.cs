using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GemElement 
{
    void Mutate();

    int GetElement()
    {
        return Random.Range(0, 4);
    }
}
