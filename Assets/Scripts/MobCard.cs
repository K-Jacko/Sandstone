using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card,", menuName = "Card")]
public class MobCard : ScriptableObject
{
    public new string name;
    public GameObject entity;
    public int creditCost;
    public float weight;
    public GemElement GemElement;
    public int health;
    public Material mobMaterial;

    public MobCard(GemElement gemElement)
    {
        GemElement = gemElement;
    }
}
