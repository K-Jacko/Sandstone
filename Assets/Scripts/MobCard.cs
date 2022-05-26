using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card,", menuName = "Card")]
public class MobCard : ScriptableObject
{
    public enum MovementType {Flying, Walking}

    public MovementType movementType;
    public new string name;
    public GameObject entity;
    
    public int creditCost;
    public float weight;
    
    public GemElement GemElement; //refactor this. You need to think about what needs to change in mutations
    public Material mobMaterial;

    public Ability ability;

    public int Stamina;
    public int Power;
    public int Agility;
    public int Focus;
}
