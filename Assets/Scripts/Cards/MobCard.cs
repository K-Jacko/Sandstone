using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card,", menuName = "Mob Card")]
public class MobCard : ScriptableObject
{
    public enum MovementType {Flying, Walking}

    public MovementType movementType;
    public new string name;
    public int creditCost;
    public float weight;
    public Material mobMaterial;
    public Ability ability;
    public Stats stats;

    public MobCard(Stats stats)
    {
        this.stats = stats;
    }
    
}
