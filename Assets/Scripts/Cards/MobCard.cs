using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card,", menuName = "Mob Card")]
public class MobCard : ScriptableObject
{
    

    public Monster.MovementType movementType;
    public string name;
    public float spawnDistance;
    public int creditCost;
    public Ability ability;
    public Stats stats;

    public MobCard(Stats stats)
    {
        this.stats = stats;
    }
    
}
