using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDirector : Director
{
    public GameObject Player { get; private set; }
    public static StageDirector Instance { get; private set; }

    private Director[] combatDirectors;

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.Find("Player");
        Instance = this;
        LoadMonsters();
        //Collect directors better
        combatDirectors = FindObjectsOfType<Director>();
        foreach (var combatDirector in combatDirectors)
        {
            combatDirector.Init();
        }

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
