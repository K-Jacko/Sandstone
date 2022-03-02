using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDirector : MonoBehaviour
{
    [SerializeField]
    public static GameObject Player;
    // Start is called before the first frame update
    public static GameObject GetPlayer()
    {
        Player = GameObject.Find("Player");
        return Player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
