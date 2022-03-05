using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDirector : MonoBehaviour
{
    public static GameObject Player;
    // Start is called before the first frame update
    public static GameObject GetPlayer()
    {
        Player = GameObject.Find("Player");
        return Player;
    }

    List<GemElement> GetValidElements()
    {
        //Validate all elements against the coEfficient 
        return new List<GemElement>
        {
            new Inferno(),
            new Terra(),
            new Aqua()
        };
    }

    public GemElement GetTopWeightedElement()
    {
        var rndInt = Random.Range(0, 3);
        return GetValidElements()[rndInt];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
