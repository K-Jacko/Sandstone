using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    public float wallet;
    public MobCard[] monsters;
    public float creditMultiplier;
    public float coEef;

    private void Awake()
    {
        TimeManager.OnMinChanged += Tick;
    }

    public virtual void Init()
    {
        
    }

    //This needs to be where the MobCards are sorted into possible Spawns based on The coEf
    protected void LoadMonsters()
    {
        monsters = Resources.LoadAll<MobCard>("Mobs");
    }

    protected virtual void Tick()
    {
        print("tick");
        coEef += 1f;
        
    }
    
    private void OnDisable()
    {
        TimeManager.OnMinChanged -= Tick;
    }
    
}
